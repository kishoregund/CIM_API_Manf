using Domain.Entities;
using Domain.Views;
using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Background service that monitors instrument warranty expiration.
    /// Sends notifications to distributors 2 months (60 days) before warranty ends.
    /// Automatically creates AMC records for instruments with expiring warranties.
    /// </summary>
    public class InstrumentWarrantyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InstrumentWarrantyBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Interval between checks (default: 24 hours)
        /// </summary>
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        /// <summary>
        /// Time of day to run the check (default: 3 AM)
        /// </summary>
        private readonly TimeOnly _scheduledTime = new TimeOnly(3, 0, 0);

        /// <summary>
        /// Days before warranty expiration to send notification (default: 60 days / 2 months)
        /// </summary>
        private readonly int _daysBeforeExpiration = 60;

        public InstrumentWarrantyBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<InstrumentWarrantyBackgroundService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Main execution method for the background service
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[InstrumentWarrantyBackgroundService] Starting instrument warranty monitoring service");

            // Wait until the scheduled time on the first run
            var delayToFirstRun = GetDelayToScheduledTime();
            _logger.LogInformation($"[InstrumentWarrantyBackgroundService] Next check scheduled for {DateTime.Now.Add(delayToFirstRun):yyyy-MM-dd HH:mm:ss}");

            try
            {
                await Task.Delay(delayToFirstRun, stoppingToken);

                // Continue running at scheduled intervals
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation($"[InstrumentWarrantyBackgroundService] Starting warranty expiration check at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        await CheckAndNotifyWarrantyExpirationAsync(stoppingToken);
                        _logger.LogInformation($"[InstrumentWarrantyBackgroundService] Warranty expiration check completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[InstrumentWarrantyBackgroundService] Error during warranty check: {ex.Message}");
                    }

                    // Wait for the next scheduled time
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("[InstrumentWarrantyBackgroundService] Background service is stopping");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[InstrumentWarrantyBackgroundService] Unexpected error in background service: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks for instruments with expiring warranties and sends notifications
        /// Also creates AMC records for instruments with expiring warranties
        /// </summary>
        private async Task CheckAndNotifyWarrantyExpirationAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var commonMethods = scope.ServiceProvider.GetRequiredService<CommonMethods>();

                try
                {
                    // Get all customer instruments with expiring warranties
                    var expiringWarranties = await GetInstrumentsWithExpiringWarrantyAsync(context);

                    if (expiringWarranties == null || expiringWarranties.Count == 0)
                    {
                        _logger.LogInformation("[InstrumentWarrantyBackgroundService] No instruments with expiring warranty found");
                        return;
                    }

                    _logger.LogInformation($"[InstrumentWarrantyBackgroundService] Found {expiringWarranties.Count} instruments with expiring warranty");

                    foreach (var custInstrument in expiringWarranties)
                    {
                        try
                        {
                            // Create AMC record for the instrument
                            await CreateAmcForInstrumentAsync(custInstrument, context, cancellationToken);

                            // Send notification to distributor
                            await NotifyDistributorForWarrantyExpirationAsync(custInstrument, context, commonMethods, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"[CheckAndNotifyWarrantyExpirationAsync] Error processing instrument {custInstrument.InstrumentId}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[InstrumentWarrantyBackgroundService] Error in CheckAndNotifyWarrantyExpirationAsync: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets all customer instruments with warranties expiring within specified days
        /// </summary>
        private async Task<List<CustomerInstrument>> GetInstrumentsWithExpiringWarrantyAsync(ApplicationDbContext context)
        {
            try
            {
                var expiringInstruments = new List<CustomerInstrument>();

                // Get all customer instruments with warranty
                var customerInstruments = await context.CustomerInstrument
                    .Where(x => x.Warranty && !x.IsDeleted && x.IsActive)
                    .ToListAsync();

                var today = DateTime.Now.Date;
                var notificationDate = today.AddDays(_daysBeforeExpiration);

                foreach (var custInstrument in customerInstruments)
                {
                    // Parse the warranty end date (format: dd/MM/yyyy)
                    if (DateTime.TryParseExact(custInstrument.WrntyEnDt, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime endDate))
                    {
                        // Check if warranty ends within the notification window
                        // And hasn't already expired
                        if (endDate >= today && endDate <= notificationDate)
                        {
                            expiringInstruments.Add(custInstrument);
                        }
                    }
                }

                return expiringInstruments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetInstrumentsWithExpiringWarrantyAsync] Error: {ex.Message}");
                return new List<CustomerInstrument>();
            }
        }

        /// <summary>
        /// Creates an AMC record for an instrument with expiring warranty
        /// </summary>
        private async Task CreateAmcForInstrumentAsync(
            CustomerInstrument custInstrument,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get site information
                var site = await context.Site.FirstOrDefaultAsync(x => x.Id == custInstrument.CustSiteId, cancellationToken);
                if (site == null)
                {
                    _logger.LogWarning($"[CreateAmcForInstrumentAsync] Site not found for customer instrument {custInstrument.Id}");
                    return;
                }

                var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == site.CustomerId, cancellationToken);
                if (customer == null)
                {
                    _logger.LogWarning($"[CreateAmcForInstrumentAsync] Customer not found for site {site.Id}");
                    return;
                }

                // Check if AMC already exists for this instrument
                var existingAmc = await context.AMC
                    .FirstOrDefaultAsync(x =>
                        x.CustSite == custInstrument.CustSiteId &&
                        !x.IsDeleted,
                        cancellationToken);

                if (existingAmc != null)
                {
                    _logger.LogInformation($"[CreateAmcForInstrumentAsync] AMC already exists for instrument {custInstrument.InstrumentId}");
                    return;
                }

                // Parse warranty end date to add one year for AMC
                DateTime amcEndDate = DateTime.Now;
                if (DateTime.TryParseExact(custInstrument.WrntyEnDt, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime parsedWarrantyEndDate))
                {
                    amcEndDate = parsedWarrantyEndDate.AddYears(1);
                }

                // Create new AMC record
                var newAmc = new AMC
                {
                    Id = Guid.NewGuid(),
                    BillTo = customer.Id,
                    CustSite = site.Id,
                    ServiceQuote = $"WRN-{custInstrument.InstrumentId:N}".Substring(0, 20), // Generate unique quote number
                    SDate = custInstrument.WrntyEnDt, // Start date from instrument warranty end date
                    EDate = amcEndDate.ToString("dd/MM/yyyy"), // End date: warranty end date + 1 year
                    Project = $"Warranty for {customer.CustName}",
                    ServiceType = GetDefaultServiceType(context).ToString(), // Convert to string as ServiceType is string field
                    BrandId = GetDefaultBrandForSite(context, site.Id),
                    CurrencyId = custInstrument.CurrencyId,
                    Zerorate = custInstrument.Cost ?? 0,
                    BaseCurrencyAmt = custInstrument.BaseCurrencyAmt ?? 1,
                    IsMultipleBreakdown = false,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"), // System user
                    CreatedOn = DateTime.Now,
                    UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    UpdatedOn = DateTime.Now
                };

                await context.AMC.AddAsync(newAmc, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"[CreateAmcForInstrumentAsync] Created AMC {newAmc.ServiceQuote} for instrument {custInstrument.InstrumentId}");

                // Create AMCInstrument relationship
                var amcInstrument = new AMCInstrument
                {
                    Id = Guid.NewGuid(),
                    AMCId = newAmc.Id,
                    InstrumentId = custInstrument.InstrumentId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    UpdatedOn = DateTime.Now
                };

                await context.AMCInstrument.AddAsync(amcInstrument, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"[CreateAmcForInstrumentAsync] Created AMC-Instrument link for AMC {newAmc.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateAmcForInstrumentAsync] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends notification to distributor about instrument warranty expiration
        /// </summary>
        private async Task NotifyDistributorForWarrantyExpirationAsync(
            CustomerInstrument custInstrument,
            ApplicationDbContext context,
            CommonMethods commonMethods,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get site information
                var site = await context.Site.FirstOrDefaultAsync(x => x.Id == custInstrument.CustSiteId, cancellationToken);
                if (site == null) return;

                var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == site.CustomerId, cancellationToken);
                var distributor = await context.Distributor.FirstOrDefaultAsync(x => x.Id == site.DistId, cancellationToken);

                if (distributor == null) return;

                // Get distributor RDTSP contacts
                var distributorContacts = await context.VW_UserProfile
                    .Where(x => x.SegmentCode == "RDTSP" && x.EntityParentId == distributor.Id)
                    .ToListAsync(cancellationToken);

                if (distributorContacts.Count == 0)
                {
                    _logger.LogWarning($"[NotifyDistributorForWarrantyExpirationAsync] No RDTSP contacts found for distributor {distributor.Id}");
                    return;
                }

                // Get instrument details
                var instrument = await context.Instrument.FirstOrDefaultAsync(x => x.Id == custInstrument.InstrumentId, cancellationToken);

                // Calculate days remaining
                if (!DateTime.TryParseExact(custInstrument.WrntyEnDt, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime warrantyEndDate))
                    return;

                var daysRemaining = (int)(warrantyEndDate.Date - DateTime.Now.Date).TotalDays;

                // Check if notification already sent today
                var existingNotification = await context.Notifications
                    .FirstOrDefaultAsync(x =>
                        x.Remarks.Contains($"warranty") &&
                        x.Remarks.Contains(custInstrument.InstrumentId.ToString()) &&
                        x.CreatedOn.Date == DateTime.Now.Date,
                        cancellationToken);

                if (existingNotification != null)
                {
                    _logger.LogInformation($"[NotifyDistributorForWarrantyExpirationAsync] Notification already sent today for instrument {custInstrument.InstrumentId}");
                    return;
                }

                //var adminId = await context.Roles
                //    .Where(x => x.Name.ToUpper() == "ADMIN")
                //    .Select(x => x.Id)
                //    .FirstOrDefaultAsync(cancellationToken);    
                // Create notifications for each distributor contact
                foreach (var contact in distributorContacts)
                {
                    try
                    {
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Warranty expiration notice: Instrument at {site.CustRegName} ({customer?.CustName}) warranty expires in {daysRemaining} days ({custInstrument.WrntyEnDt}). Auto-created AMC has been prepared.",
                            RoleId = contact.RoleId,
                            RaisedBy = "System",
                            UserId = contact.UserId,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                            UpdatedOn = DateTime.Now
                        };

                        await context.Notifications.AddAsync(notification, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        _logger.LogInformation($"[NotifyDistributorForWarrantyExpirationAsync] Created notification for user {contact.UserId}");

                        // Send email notification
                        await SendWarrantyExpirationEmailAsync(contact, custInstrument, site, customer, instrument, daysRemaining, commonMethods);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[NotifyDistributorForWarrantyExpirationAsync] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[NotifyDistributorForWarrantyExpirationAsync] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification about warranty expiration
        /// </summary>
        private async Task SendWarrantyExpirationEmailAsync(
            VW_UserProfile recipient,
            CustomerInstrument custInstrument,
            Site site,
            Customer customer,
            Instrument instrument,
            int daysRemaining,
            CommonMethods commonMethods)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email))
                {
                    _logger.LogWarning($"[SendWarrantyExpirationEmailAsync] No email for user {recipient.UserId}");
                    return;
                }

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>?? Instrument Warranty Expiration Notice</h3>
                    
                    <div style='background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 10px 0;'>
                        <p><strong>ALERT:</strong> An instrument warranty is expiring soon!</p>
                    </div>
                    
                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Customer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customer?.CustName ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Site Location:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{site.CustRegName}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Instrument Type:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{instrument?.InsType ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Serial Number:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{instrument?.SerialNos ?? "N/A"}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Warranty Start Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{custInstrument.WrntyStDt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Warranty End Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #dc3545;'>{custInstrument.WrntyEnDt}</strong></td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Days Remaining:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: {(daysRemaining <= 30 ? "#dc3545" : "#ffc107")};'>{daysRemaining} days</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Installation Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{custInstrument.InstallDt ?? "N/A"}</td>
                        </tr>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>? Automated Actions Taken:</strong></p>
                        <ul>
                            <li>? AMC record has been automatically created for this instrument</li>
                            <li>? You can now proceed with warranty renewal/AMC setup</li>
                            <li>? Contact the customer to discuss extended warranty options</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Recommended Actions:</strong></p>
                        <ul>
                            <li>Review the auto-created AMC record in the system</li>
                            <li>Contact customer to discuss warranty extension or new AMC</li>
                            <li>Update customer about maintenance coverage options</li>
                            <li>Plan service continuity before warranty expires</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification sent {daysRemaining} days before warranty expiration.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>An AMC record has been automatically created to facilitate the warranty renewal process.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Warranty Expiration Alert - Instrument at {site.CustRegName} expires in {daysRemaining} days";

                commonMethods.SendEmailMethod(recipient.Email, emailBody, subject);

                _logger.LogInformation($"[SendWarrantyExpirationEmailAsync] Email sent to {recipient.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[SendWarrantyExpirationEmailAsync] Error sending email: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the default service type (usually "Warranty" list item)
        /// </summary>
        private Guid GetDefaultServiceType(ApplicationDbContext context)
        {
            try
            {             

                // Default to any available service type
                var anyServiceType = context.VW_ListItems
                    .FirstOrDefault(x => x.ItemCode == "AMC" && x.ListCode == "SERTY");

                if (anyServiceType != null)
                    return anyServiceType.ListTypeItemId;

                return Guid.Empty; // Will be handled in validation
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetDefaultServiceType] Error: {ex.Message}");
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the default brand for a site
        /// </summary>
        private Guid GetDefaultBrandForSite(ApplicationDbContext context, Guid siteId)
        {
            try
            {
                // Get customer for the site, then their default brand
                var site = context.Site.FirstOrDefault(x => x.Id == siteId);
                if (site == null) return Guid.Empty;

                var customer = context.Customer.FirstOrDefault(x => x.Id == site.CustomerId);
                if (customer == null) return Guid.Empty;

                // Get any brand from business unit (simplified approach)
                var brand = context.Brand.FirstOrDefault();
                return brand?.Id ?? Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Calculates the time delay to the next scheduled check time
        /// </summary>
        private TimeSpan GetDelayToScheduledTime()
        {
            var now = DateTime.Now;
            var scheduledDateTime = DateTime.Today.Add(_scheduledTime.ToTimeSpan());

            // If scheduled time has already passed today, schedule for tomorrow
            if (now > scheduledDateTime)
            {
                scheduledDateTime = scheduledDateTime.AddDays(1);
            }

            var delay = scheduledDateTime - now;
            return delay > TimeSpan.Zero ? delay : TimeSpan.FromSeconds(1);
        }
    }
}
