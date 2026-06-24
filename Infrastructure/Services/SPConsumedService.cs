using Application.Features.ServiceReports;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class SPConsumedService(ApplicationDbContext Context, ICurrentUserService currentUserService, CommonMethods commonMethods, ILogger<SPConsumedService> logger) : ISPConsumedService
    {

        public async Task<SPConsumed> GetSPConsumedAsync(Guid id)
            => await Context.SPConsumed.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SPConsumed>> GetSPConsumedBySRPIdAsync(Guid serviceReportId)
       => await Context.SPConsumed.Where(x=>x.ServiceReportId == serviceReportId).ToListAsync();

        public async Task<Guid> CreateSPConsumedAsync(SPConsumed SPConsumed)
        {
            SPConsumed.CreatedOn = DateTime.Now;
            SPConsumed.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedOn = DateTime.Now;

            await Context.SPConsumed.AddAsync(SPConsumed);
            await Context.SaveChangesAsync();

            // Send real-time notification to distributor
            await NotifyDistributorForSparePartsConsumedAsync(SPConsumed);

            return SPConsumed.Id;
        }

        public async Task<bool> DeleteSPConsumedAsync(Guid id)
        {

            var deletedEngAction = await Context
                .SPConsumed.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSPConsumedAsync(SPConsumed SPConsumed)
        {
            SPConsumed.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPConsumed.UpdatedOn = DateTime.Now;

            Context.Entry(SPConsumed).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            // Send real-time notification to distributor
            await NotifyDistributorForSparePartsConsumedAsync(SPConsumed);

            return SPConsumed.Id;
        }

        /// <summary>
        /// Notifies distributor in real-time when spare parts are consumed
        /// </summary>
        private async Task NotifyDistributorForSparePartsConsumedAsync(SPConsumed sparePartConsumed)
        {
            try
            {
                // Get service report details
                var serviceReport = await Context.ServiceReport
                    .FirstOrDefaultAsync(x => x.Id == sparePartConsumed.ServiceReportId);

                if (serviceReport == null) return;

                // Get service request details
                var serviceRequest = await Context.ServiceRequest
                    .FirstOrDefaultAsync(x => x.Id == serviceReport.ServiceRequestId);

                if (serviceRequest == null) return;

                // Get site and distributor
                var site = await Context.Site
                    .FirstOrDefaultAsync(x => x.Id == serviceRequest.SiteId);
                var customer = site != null ? await Context.Customer
                    .FirstOrDefaultAsync(x => x.Id == site.CustomerId) : null;
                var distributor = site != null ? await Context.Distributor
                    .FirstOrDefaultAsync(x => x.Id == site.DistId) : null;

                if (distributor == null) return;

                // Get distributor contacts (RDTSP - Regional Distributors)
                var distributorContacts = await Context.VW_UserProfile
                    .Where(x => x.SegmentCode == "RDTSP" && x.EntityParentId == distributor.Id)
                    .ToListAsync();

                if (distributorContacts.Count == 0) return;

                // Create notifications for each distributor contact
                foreach (var contact in distributorContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Spare part consumed: {sparePartConsumed.PartNo} ({sparePartConsumed.ItemDesc}) - Qty: {sparePartConsumed.QtyConsumed} at {site?.CustRegName ?? "N/A"} ({customer?.CustName ?? "N/A"}). Service Report: {serviceReport.ServiceReportNo}",
                            RoleId = contact.RoleId,
                            RaisedBy = currentUserService.GetUserId(),
                            UserId = contact.UserId,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = Guid.Parse(currentUserService.GetUserId()),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = Guid.Parse(currentUserService.GetUserId()),
                            UpdatedOn = DateTime.Now
                        };

                        await Context.Notifications.AddAsync(notification);
                        await Context.SaveChangesAsync();

                        logger.LogInformation($"[SPConsumedService] Created notification for user {contact.UserId} - Spare part: {sparePartConsumed.PartNo}");

                        // Send email notification
                        await SendSparePartConsumedEmailAsync(contact, sparePartConsumed, serviceReport, serviceRequest, site, customer);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"[SPConsumedService] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"[SPConsumedService] Error in NotifyDistributorForSparePartsConsumedAsync: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification about spare part consumed
        /// </summary>
        private async Task SendSparePartConsumedEmailAsync(
            VW_UserProfile recipient,
            SPConsumed sparePartConsumed,
            ServiceReport serviceReport,
            ServiceRequest serviceRequest,
            Site site,
            Customer customer)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email))
                {
                    logger.LogWarning($"[SendSparePartConsumedEmailAsync] No email for user {recipient.UserId}");
                    return;
                }

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>📦 Spare Part Consumed Notification</h3>
                    
                    <div style='background-color: #d1ecf1; padding: 15px; border-left: 4px solid #17a2b8; margin: 10px 0;'>
                        <p><strong>INFO:</strong> A spare part has been consumed during service.</p>
                    </div>
                    
                    <h4>Service Details:</h4>
                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Report #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportNo}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Request #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqNo}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Customer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customer?.CustName ?? ""}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Site:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{site?.CustRegName ?? ""}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportDate:dd/MM/yyyy}</td>
                        </tr>
                    </table>
                    
                    <h4>Spare Part Details:</h4>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <thead>
                            <tr style='background-color: #007bff; color: white;'>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Part Number</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Description</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Quantity Consumed</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>HS Code</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartConsumed.PartNo}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartConsumed.ItemDesc}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartConsumed.QtyConsumed}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartConsumed.HscCode}</td>
                            </tr>
                        </tbody>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Recommended Actions:</strong></p>
                        <ul>
                            <li>Review inventory levels for this spare part</li>
                            <li>Ensure adequate stock is maintained for future service calls</li>
                            <li>Coordinate with logistics for replacement stock</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when spare parts are consumed during service.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Spare Part Consumed - {sparePartConsumed.PartNo} ({sparePartConsumed.ItemDesc})";

                commonMethods.SendEmailMethod(recipient.Email, emailBody, subject);

                logger.LogInformation($"[SendSparePartConsumedEmailAsync] Email sent to {recipient.Email}");
            }
            catch (Exception ex)
            {
                logger.LogError($"[SendSparePartConsumedEmailAsync] Error sending email: {ex.Message}");
            }
        }

    }
}
