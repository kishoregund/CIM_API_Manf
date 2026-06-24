using Application.Features.ServiceReports;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceReports.Responses;
using Application.Features.Spares.Responses;
using Domain.Views;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Application.Features.Masters.Responses;

namespace Infrastructure.Services
{
    public class SPRecommendedService(ApplicationDbContext Context, ICurrentUserService currentUserService, IConfiguration configuration) : ISPRecommendedService
    {

        public async Task<SPRecommended> GetSPRecommendedAsync(Guid id)
            => await Context.SPRecommended.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SPRecommended>> GetSPRecommendedBySRPIdAsync(Guid serviceReportId)
            => await Context.SPRecommended.Where(p => p.ServiceReportId == serviceReportId).ToListAsync();

        public async Task<List<VW_SparesRecommended>> GetSPRecommendedGridAsync(string buId, string brandId)
        {
            List<VW_SparesRecommended> sparesRecom = new();
            List<VW_SparesRecommended> lstSparePartsRecommended = new();
            var userProfile = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            if (userProfile == null || userProfile.FirstName.Equals("Admin"))
            {
                return await Context.VW_SparesRecommended.OrderBy(x => x.QtyRecommended).ToListAsync();
            }

            sparesRecom = await Context.VW_SparesRecommended.Where(x => !x.IsDeleted).ToListAsync();
            if (!string.IsNullOrEmpty(buId))
            {
                sparesRecom = sparesRecom.Where(x => x.BrandId.ToString() == brandId && x.BusinessUnitId.ToString() == buId).ToList();
            }

            var serRequests = new List<ServiceRequest>();
            CommonMethods commonMethods = new CommonMethods(Context, currentUserService, configuration);
            var lstRegionsProfile = commonMethods.GetDistRegionsByUserIdAsync().Result;
            if (userProfile.ContactType.ToLower() == "cs")
            {
                serRequests = Context.ServiceRequest.Where(x => x.CustId == userProfile.EntityParentId).ToList();

                foreach (var item in serRequests)
                {
                    lstSparePartsRecommended.AddRange(sparesRecom.Where(x => x.ServiceRequestId == item.Id && lstRegionsProfile.Contains(x.SiteRegion.ToString())).OrderBy(x => x.QtyRecommended).ToList());
                }
            }
            if (userProfile.ContactType.ToLower() == "dr")
            {
                serRequests = Context.ServiceRequest.Where(x => x.DistId == userProfile.EntityParentId).ToList();

                foreach (var item in serRequests)
                {
                    lstSparePartsRecommended.AddRange(sparesRecom.Where(x => x.ServiceRequestId == item.Id && lstRegionsProfile.Contains(x.DefDistRegionId.ToString())).OrderBy(x => x.QtyRecommended).ToList());
                }
            }
            
            return lstSparePartsRecommended;
        }



        public async Task<List<VW_Spareparts>> GetSPRecommendedBySerReqAsync(Guid serviceRequestId)
        {
            return await(from s in Context.ServiceRequest 
                               join i in Context.Instrument on s.MachinesNo equals i.Id.ToString()
                               join isp in Context.InstrumentSpares on i.Id equals isp.InstrumentId
                               join sp in Context.VW_Spareparts on isp.SparepartId equals sp.Id
                               where s.Id == serviceRequestId
                               select sp).ToListAsync();

        }

        public async Task<Guid> CreateSPRecommendedAsync(SPRecommended SPRecommended)
        {
            SPRecommended.CreatedOn = DateTime.Now;
            SPRecommended.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SPRecommended.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPRecommended.UpdatedOn = DateTime.Now;

            await Context.SPRecommended.AddAsync(SPRecommended);
            await Context.SaveChangesAsync();

            // Send real-time notification to distributor
            await NotifyDistributorForSparePartsRecommendedAsync(SPRecommended);

            // Send real-time notification to customer site contacts
            await NotifyCustomerForSparePartsRecommendedAsync(SPRecommended);

            return SPRecommended.Id;
        }

        public async Task<bool> DeleteSPRecommendedAsync(Guid id)
        {

            var deletedEngAction = await Context
                .SPRecommended.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSPRecommendedAsync(SPRecommended SPRecommended)
        {
            SPRecommended.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SPRecommended.UpdatedOn = DateTime.Now;

            Context.Entry(SPRecommended).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            // Send real-time notification to distributor
            await NotifyDistributorForSparePartsRecommendedAsync(SPRecommended);

            // Send real-time notification to customer site contacts
            await NotifyCustomerForSparePartsRecommendedAsync(SPRecommended);

            return SPRecommended.Id;
        }

        /// <summary>
        /// Notifies distributor in real-time when spare parts are recommended
        /// </summary>
        private async Task NotifyDistributorForSparePartsRecommendedAsync(SPRecommended sparePartRecommended)
        {
            try
            {
                // Get service report details
                var serviceReport = await Context.ServiceReport
                    .FirstOrDefaultAsync(x => x.Id == sparePartRecommended.ServiceReportId);

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
                            Remarks = $"Spare part recommended: {sparePartRecommended.PartNo} ({sparePartRecommended.ItemDesc}) - Qty: {sparePartRecommended.QtyRecommended} for {site?.CustRegName ?? "N/A"} ({customer?.CustName ?? "N/A"}). Service Report: {serviceReport.ServiceReportNo}",
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

                        // Send email notification
                        await SendSparePartRecommendedEmailAsync(contact, sparePartRecommended, serviceReport, serviceRequest, site, customer);
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue processing other contacts
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - don't want to block the main operation
            }
        }

        /// <summary>
        /// Sends email notification about spare part recommended
        /// </summary>
        private async Task SendSparePartRecommendedEmailAsync(
            VW_UserProfile recipient,
            SPRecommended sparePartRecommended,
            ServiceReport serviceReport,
            ServiceRequest serviceRequest,
            Site site,
            Customer customer)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email)) return;

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>💡 Spare Part Recommended Notification</h3>
                    
                    <div style='background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 10px 0;'>
                        <p><strong>ACTION REQUIRED:</strong> Service engineer has recommended a spare part.</p>
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
                    
                    <h4>Recommended Spare Part:</h4>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <thead>
                            <tr style='background-color: #ffc107; color: black;'>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Part Number</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Description</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>Recommended Quantity</th>
                                <th style='padding: 8px; border: 1px solid #ddd;'>HS Code</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartRecommended.PartNo}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartRecommended.ItemDesc}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartRecommended.QtyRecommended}</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{sparePartRecommended.HscCode}</td>
                            </tr>
                        </tbody>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>✓ Engineer's Recommendation:</strong></p>
                        <p>The service engineer has recommended this spare part based on the service assessment. This part may be needed for optimal equipment performance or as preventive maintenance.</p>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Recommended Actions:</strong></p>
                        <ul>
                            <li>Review engineer's recommendation</li>
                            <li>Contact customer to discuss spare part needs</li>
                            <li>Prepare quotation for recommended spare part</li>
                            <li>Plan for procurement if part needs to be ordered</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when service engineers recommend spare parts.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Spare Part Recommended - {sparePartRecommended.PartNo} ({sparePartRecommended.ItemDesc})";

                var cm = new CommonMethods(Context, currentUserService, configuration);
                cm.SendEmailMethod(recipient.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                // Log but don't throw
            }
        }

        /// <summary>
        /// Notifies customer site contacts in real-time when spare parts are recommended
        /// </summary>
        private async Task NotifyCustomerForSparePartsRecommendedAsync(SPRecommended sparePartRecommended)
        {
            try
            {
                // Get service report details
                var serviceReport = await Context.ServiceReport
                    .FirstOrDefaultAsync(x => x.Id == sparePartRecommended.ServiceReportId);

                if (serviceReport == null) return;

                // Get service request details
                var serviceRequest = await Context.ServiceRequest
                    .FirstOrDefaultAsync(x => x.Id == serviceReport.ServiceRequestId);

                if (serviceRequest == null) return;

                var site = await Context.Site.FirstOrDefaultAsync(x => x.Id == serviceRequest.SiteId);

                // Get site contacts (active only)
                var siteContacts = await Context.VW_UserProfile
                    .Where(x => x.EntityChildId == serviceRequest.SiteId && x.EntityParentId == serviceRequest.CustId)
                    .ToListAsync();

                if (siteContacts.Count == 0) return;

                // Get engineer details
                var engineer = await Context.RegionContact
                    .FirstOrDefaultAsync(x => x.Id == serviceRequest.AssignedTo);

                // Create notifications for each site contact
                foreach (var contact in siteContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Spare part recommended: {sparePartRecommended.PartNo} ({sparePartRecommended.ItemDesc}) - Qty: {sparePartRecommended.QtyRecommended} for {site?.CustRegName ?? "N/A"}. Engineer: {engineer?.FirstName} {engineer?.LastName}. Service Report: {serviceReport.ServiceReportNo}",
                            RoleId = contact.RoleId,
                            RaisedBy = currentUserService.GetUserId(),
                            UserId = contact.UserId,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                            UpdatedOn = DateTime.Now
                        };

                        await Context.Notifications.AddAsync(notification);
                        await Context.SaveChangesAsync();

                        // Send email notification to site contact
                        await SendCustomerSparePartRecommendedEmailAsync(contact, sparePartRecommended, serviceReport, serviceRequest, engineer);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForSparePartsRecommended] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForSparePartsRecommended] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification to customer site contact about spare parts recommended
        /// </summary>
        private async Task SendCustomerSparePartRecommendedEmailAsync(
            VW_UserProfile contact,
            SPRecommended sparePartRecommended,
            ServiceReport serviceReport,
            ServiceRequest serviceRequest,            
            RegionContact engineer)
        {
            try
            {
                if (string.IsNullOrEmpty(contact.Email)) return;

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>📦 Spare Part Recommendation from Service Engineer</h3>
                    
                    <div style='background-color: #d1ecf1; padding: 15px; border-left: 4px solid #0c5460; margin: 10px 0;'>
                        <p><strong>✓ INFO:</strong> Service engineer has recommended a spare part for your equipment.</p>
                    </div>
                    
                    <h4>Service Information:</h4>
                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Request #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqNo}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Report #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportNo}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Engineer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{engineer?.FirstName} {engineer?.LastName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportDate:dd/MM/yyyy}</td>
                        </tr>                        
                    </table>
                    
                    <h4>Recommended Spare Part Details:</h4>
                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                        <thead>
                            <tr style='background-color: #d1ecf1;'>
                                <th style='padding: 10px; border: 1px solid #0c5460; text-align: left;'>Part Number</th>
                                <th style='padding: 10px; border: 1px solid #0c5460; text-align: left;'>Description</th>
                                <th style='padding: 10px; border: 1px solid #0c5460; text-align: center;'>Recommended Qty</th>
                                <th style='padding: 10px; border: 1px solid #0c5460; text-align: left;'>HS Code</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style='padding: 10px; border: 1px solid #ddd;'><strong>{sparePartRecommended.PartNo}</strong></td>
                                <td style='padding: 10px; border: 1px solid #ddd;'>{sparePartRecommended.ItemDesc}</td>
                                <td style='padding: 10px; border: 1px solid #ddd; text-align: center;'><strong>{sparePartRecommended.QtyRecommended}</strong></td>
                                <td style='padding: 10px; border: 1px solid #ddd;'>{sparePartRecommended.HscCode}</td>
                            </tr>
                        </tbody>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #d1ecf1; border-left: 4px solid #0c5460;'>
                        <p><strong>What This Means:</strong></p>
                        <p>Based on the service assessment conducted at your site, the engineer has recommended this spare part. This may be needed for:</p>
                        <ul>
                            <li>Optimal equipment performance and reliability</li>
                            <li>Preventive maintenance to avoid future breakdowns</li>
                            <li>Extended equipment lifespan</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>Next Steps:</strong></p>
                        <ul>
                            <li>✓ Review this spare part recommendation</li>
                            <li>✓ Contact your distributor or service coordinator for pricing</li>
                            <li>✓ Decide on procurement based on your maintenance schedule</li>
                            <li>✓ Keep this information for your records</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Questions?</strong></p>
                        <p>If you have any questions about this spare part recommendation or need clarification, please contact:</p>
                        <ul>
                            <li>Service Engineer: {engineer?.FirstName} {engineer?.LastName}</li>
                            <li>Your Service Coordinator or Distributor</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when service engineers recommend spare parts.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Spare Part Recommended for Your Site - {sparePartRecommended.PartNo} ({sparePartRecommended.ItemDesc})";

                var cm = new CommonMethods(Context, currentUserService, configuration);
                cm.SendEmailMethod(contact.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendCustomerSparePartRecommendedEmailAsync] Error sending email: {ex.Message}");
            }
        }



    }
}
