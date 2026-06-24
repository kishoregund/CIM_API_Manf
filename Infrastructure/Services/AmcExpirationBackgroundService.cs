using Application.Features.AMCS;
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
    /// Background service that runs automatically to check and notify about expiring AMCs.
    /// This service runs on a schedule (default: daily at 2 AM) without requiring API calls.
    /// </summary>
    public class AmcExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AmcExpirationBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Interval between checks (default: 24 hours)
        /// </summary>
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        /// <summary>
        /// Time of day to run the check (default: 2 AM)
        /// </summary>
        private readonly TimeOnly _scheduledTime = new TimeOnly(2, 0, 0);

        public AmcExpirationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<AmcExpirationBackgroundService> logger,
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
            _logger.LogInformation("[AmcExpirationBackgroundService] Starting AMC expiration notification background service");

            // Wait until the scheduled time on the first run
            var delayToFirstRun = GetDelayToScheduledTime();
            _logger.LogInformation($"[AmcExpirationBackgroundService] Next check scheduled for {DateTime.Now.Add(delayToFirstRun):yyyy-MM-dd HH:mm:ss}");

            try
            {
                await Task.Delay(delayToFirstRun, stoppingToken);

                // Continue running at scheduled intervals
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation($"[AmcExpirationBackgroundService] Starting AMC expiration check at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        await CheckAndNotifyExpiringAmcsAsync(stoppingToken);
                        _logger.LogInformation($"[AmcExpirationBackgroundService] AMC expiration check completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[AmcExpirationBackgroundService] Error during AMC expiration check: {ex.Message}");
                    }

                    // Wait for the next scheduled time
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("[AmcExpirationBackgroundService] Background service is stopping");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AmcExpirationBackgroundService] Unexpected error in background service: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks for expiring AMCs and sends notifications
        /// </summary>
        private async Task CheckAndNotifyExpiringAmcsAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var commonMethods = scope.ServiceProvider.GetRequiredService<CommonMethods>();

                try
                {
                    var expiringAmcs = await GetExpiringAmcsAsync(context, 60);

                    if (expiringAmcs == null || expiringAmcs.Count == 0)
                    {
                        _logger.LogInformation("[AmcExpirationBackgroundService] No expiring AMCs found");
                        return;
                    }

                    _logger.LogInformation($"[AmcExpirationBackgroundService] Found {expiringAmcs.Count} expiring AMCs");

                    foreach (var amc in expiringAmcs)
                    {
                        await NotifyForAmcAsync(amc, context, commonMethods, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[AmcExpirationBackgroundService] Error in CheckAndNotifyExpiringAmcsAsync: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets all AMCs expiring within specified days
        /// </summary>
        private async Task<List<AMC>> GetExpiringAmcsAsync(ApplicationDbContext context, int daysBeforeExpiry)
        {
            try
            {
                var expiringAmcs = new List<AMC>();

                // Get all active AMCs
                var activeAmcs = await context.AMC
                    .Where(x => x.IsActive && !x.IsDeleted)
                    .ToListAsync();

                var today = DateTime.Now.Date;
                var notificationDate = today.AddDays(daysBeforeExpiry);

                foreach (var amc in activeAmcs)
                {
                    // Parse the EDate (format: dd/MM/yyyy)
                    if (DateTime.TryParseExact(amc.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime endDate))
                    {
                        // Check if AMC is within 60 days of expiry and not already expired
                        if (endDate >= today && endDate <= notificationDate)
                        {
                            expiringAmcs.Add(amc);
                        }
                    }
                }

                return expiringAmcs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetExpiringAmcsAsync] Error: {ex.Message}");
                return new List<AMC>();
            }
        }

        /// <summary>
        /// Gets distributor contacts for a specific distributor
        /// </summary>
        private async Task<List<VW_UserProfile>> GetDistributorContactsAsync(
            ApplicationDbContext context,
            Guid distributorId)
        {
            try
            {
                // Get all RDTSP users (Regional Distributors) for this distributor
                var distributorContacts = await context.VW_UserProfile
                    .Where(x => x.SegmentCode == "RDTSP" &&
                               x.EntityParentId == distributorId)
                    .ToListAsync();

                return distributorContacts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetDistributorContactsAsync] Error: {ex.Message}");
                return new List<VW_UserProfile>();
            }
        }

        /// <summary>
        /// Sends notification for a specific AMC
        /// </summary>
        private async Task NotifyForAmcAsync(
            AMC amc,
            ApplicationDbContext context,
            CommonMethods commonMethods,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get site and customer details
                var site = await context.Site.FirstOrDefaultAsync(x => x.Id == amc.CustSite, cancellationToken);
                if (site == null)
                {
                    _logger.LogWarning($"[NotifyForAmcAsync] Site not found for AMC {amc.ServiceQuote}");
                    return;
                }

                var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == site.CustomerId, cancellationToken);
                var distributor = await context.Distributor.FirstOrDefaultAsync(x => x.Id == site.DistId, cancellationToken);

                if (distributor == null)
                {
                    _logger.LogWarning($"[NotifyForAmcAsync] Distributor not found for AMC {amc.ServiceQuote}");
                    return;
                }

                // Get distributor contacts
                var distributorContacts = await GetDistributorContactsAsync(context, distributor.Id);

                if (distributorContacts.Count == 0)
                {
                    _logger.LogWarning($"[NotifyForAmcAsync] No RDTSP contacts found for distributor {distributor.Id}");
                    return;
                }

                // Calculate days remaining
                if (!DateTime.TryParseExact(amc.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime endDate))
                {
                    _logger.LogError($"[NotifyForAmcAsync] Invalid date format for AMC {amc.ServiceQuote}");
                    return;
                }

                var daysRemaining = (int)(endDate.Date - DateTime.Now.Date).TotalDays;

                // Check if notification already sent today for this AMC
                var existingNotification = await context.Notifications
                    .FirstOrDefaultAsync(x =>
                        x.Remarks.Contains(amc.ServiceQuote) &&
                        x.CreatedOn.Date == DateTime.Now.Date,
                        cancellationToken);

                if (existingNotification != null)
                {
                    _logger.LogInformation($"[NotifyForAmcAsync] Notification already sent today for AMC {amc.ServiceQuote}");
                    return;
                }

                // Create in-app notifications and send emails
                foreach (var contact in distributorContacts)
                {
                    try
                    {
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"AMC {amc.ServiceQuote} for {customer?.CustName ?? "N/A"} at {site.CustRegName} expires in {daysRemaining} days",
                            RoleId = contact.RoleId,
                            RaisedBy = "System",
                            UserId = contact.UserId,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"), // System user
                            CreatedOn = DateTime.Now,
                            UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                            UpdatedOn = DateTime.Now
                        };

                        await context.Notifications.AddAsync(notification, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        _logger.LogInformation($"[NotifyForAmcAsync] Created notification for user {contact.UserId} for AMC {amc.ServiceQuote}");

                        // Send email notification
                        await SendAmcExpirationEmailAsync(contact, amc, site, customer, daysRemaining, commonMethods);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[NotifyForAmcAsync] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[NotifyForAmcAsync] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification for AMC expiration
        /// </summary>
        private async Task SendAmcExpirationEmailAsync(
            VW_UserProfile recipient,
            AMC amc,
            Site site,
            Customer customer,
            int daysRemaining,
            CommonMethods commonMethods)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email))
                {
                    _logger.LogWarning($"[SendAmcExpirationEmailAsync] No email for user {recipient.UserId}");
                    return;
                }

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>?? AMC Expiration Notice</h3>
                    
                    <div style='background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 10px 0;'>
                        <p><strong>?? ALERT:</strong> An AMC is expiring soon!</p>
                    </div>
                    
                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Quote Number:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{amc.ServiceQuote}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Customer Name:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customer?.CustName ?? "N/A"}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Site Name:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{site.CustRegName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>AMC Start Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{amc.SDate}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>AMC End Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #dc3545;'>{amc.EDate}</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Days Remaining:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: {(daysRemaining <= 30 ? "#dc3545" : "#ffc107")};'>{daysRemaining} days</strong></td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Type:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{amc.ServiceType}</td>
                        </tr>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>Recommended Actions:</strong></p>
                        <ul>
                            <li>Review AMC status and coverage</li>
                            <li>Prepare renewal documentation if needed</li>
                            <li>Contact customer to discuss AMC renewal options</li>
                            <li>Plan for service continuity post-expiration</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Action Required:</strong></p>
                        <p>Please log into the CIM dashboard to review this AMC and take necessary action.</p>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification sent {daysRemaining} days before AMC expiration.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email and you will not receive a response. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"AMC Expiration Alert - {amc.ServiceQuote} expires in {daysRemaining} days";

                commonMethods.SendEmailMethod(recipient.Email, emailBody, subject);

                _logger.LogInformation($"[SendAmcExpirationEmailAsync] Email sent to {recipient.Email} for AMC {amc.ServiceQuote}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[SendAmcExpirationEmailAsync] Error sending email to {recipient.Email}: {ex.Message}");
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
