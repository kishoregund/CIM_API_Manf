using Application.Features.ServiceRequests;
using Domain.Entities;
using Application.Features.Identity.Users;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceRequests.Responses;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Domain.Views;

namespace Infrastructure.Services
{
    public class SREngActionService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : ISREngActionService
    {

        public async Task<SREngAction> GetSREngActionAsync(Guid id)
            => await context.SREngAction.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        public async Task<List<SREngAction>> GetSREngActionEntityBySRIdAsync(Guid serviceRequestId)
        => await context.SREngAction.Where(p => p.ServiceRequestId == serviceRequestId && !p.IsDeleted).ToListAsync();

        public async Task<List<SREngActionResponse>> GetSREngActionBySRIdAsync(Guid serviceRequestId)
        { 
           var actions = await context.SREngAction.Where(p => p.ServiceRequestId == serviceRequestId).ToListAsync();
            List<SREngActionResponse> srEngActionResponses = new();
            foreach (SREngAction engAct in actions)
            {
                var mSREngineerAction = new SREngActionResponse();
                var eng = context.RegionContact.FirstOrDefault(x => x.Id == engAct.EngineerId);           
                mSREngineerAction.Id = engAct.Id;
                mSREngineerAction.Comments = engAct.Comments;
                mSREngineerAction.IsActive = engAct.IsActive;
                mSREngineerAction.ActionDate = engAct.ActionDate;
                mSREngineerAction.Actiontaken = engAct.Actiontaken;
                mSREngineerAction.ActiontakenName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == engAct.Actiontaken)?.ItemName;
                mSREngineerAction.EngineerId = engAct.EngineerId;
                mSREngineerAction.EngineerName = eng.FirstName + " " + eng.LastName;
                mSREngineerAction.TeamviewRecording = engAct.TeamviewRecording;
                mSREngineerAction.ServiceRequestId = engAct.ServiceRequestId;

                srEngActionResponses.Add(mSREngineerAction);
            }
            return srEngActionResponses.ToList();
        }

        public async Task<Guid> CreateSREngActionAsync(SREngAction EngAction)
        {
            EngAction.CreatedOn = DateTime.Now;
            EngAction.UpdatedOn = DateTime.Now;
            EngAction.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            EngAction.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.SREngAction.AddAsync(EngAction);
            await context.SaveChangesAsync();

            // Notify customer when engineer takes action
            try
            {
                var serviceRequest = await context.ServiceRequest.FirstOrDefaultAsync(x => x.Id == EngAction.ServiceRequestId);
                var engineer = await context.RegionContact.FirstOrDefaultAsync(x => x.Id == EngAction.EngineerId);
                
                if (serviceRequest != null && engineer != null)
                {
                    await NotifyCustomerForEngineerActionAsync(serviceRequest, EngAction.Actiontaken, EngAction.Comments, engineer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CreateSREngActionAsync] Error notifying customer: {ex.Message}");
            }

            return EngAction.Id;
        }

        public async Task<bool> DeleteSREngActionAsync(Guid id)
        {

            var deletedEngAction = await context
                .SREngAction.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateSREngActionAsync(SREngAction EngAction)
        {
            EngAction.UpdatedOn = DateTime.Now;
            EngAction.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(EngAction).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return EngAction.Id;
        }

        /// <summary>
        /// Notifies customer site contact when engineer accepts/rejects service request
        /// </summary>
        private async Task NotifyCustomerForEngineerActionAsync(Domain.Entities.ServiceRequest serviceRequest, string actionTaken, string comments, RegionContact engineer)
        {
            try
            {
                if (serviceRequest == null || engineer == null) return;

                var site = await context.Site.FirstOrDefaultAsync(x => x.Id == serviceRequest.SiteId);
                if (site == null) return;

                var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == site.CustomerId);
                var siteContacts = await context.VW_UserProfile.Where(x => x.SegmentCode == "RCUST"
                && x.EntityChildId == serviceRequest.SiteId && x.EntityParentId == serviceRequest.CustId).ToListAsync();

                if (siteContacts.Count == 0) return;

                var actionName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == actionTaken)?.ItemName ?? "PENDING";

                foreach (var contact in siteContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Engineer {engineer.FirstName} {engineer.LastName} has {actionName.ToLower()} Service Request {serviceRequest.SerReqNo}. {(!string.IsNullOrEmpty(comments) ? $"Comments: {comments}" : "")}",
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

                        await context.Notifications.AddAsync(notification);
                        await context.SaveChangesAsync();

                        // Send email notification
                        await SendCustomerEngineerActionEmailAsync(contact, engineer, serviceRequest, site, customer, actionName, comments);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForEngineerAction] Error notifying contact {contact.ContactId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForEngineerAction] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification to customer for engineer actions
        /// </summary>
        private async Task SendCustomerEngineerActionEmailAsync(
            VW_UserProfile contact,
            RegionContact engineer,
            Domain.Entities.ServiceRequest serviceRequest,
            Site site,
            Customer customer,
            string actionName,
            string comments)
        {
            try
            {
                if (string.IsNullOrEmpty(contact.Email)) return;

                var actionColor = actionName?.ToUpper() == "ACCEPTED" ? "#28a745" : "#dc3545";
                var actionIcon = actionName?.ToUpper() == "ACCEPTED" ? "✓" : "✗";

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>{actionIcon} Engineer Response - Service Request Status</h3>
                    
                    <div style='background-color: #d4edda; padding: 15px; border-left: 4px solid #28a745; margin: 10px 0;'>
                        <p><strong>Engineer Response:</strong> {engineer.FirstName} {engineer.LastName} has <strong style='color: {actionColor};'>{actionName}</strong> your service request.</p>
                    </div>
                    
                    <h4>Service Request Details:</h4>
                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Request #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqNo}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Engineer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{engineer.FirstName} {engineer.LastName}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Action Taken:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: {actionColor};'>{actionName}</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Customer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customer?.CustName ?? "N/A"}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Site Location:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{site?.CustRegName ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Request Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqDate}</td>
                        </tr>
                    </table>

                    {(!string.IsNullOrEmpty(comments) ? $@"
                    <h4>Engineer Comments:</h4>
                    <div style='padding: 15px; background-color: #f0f0f0; border-left: 4px solid #007bff; margin-bottom: 20px;'>
                        <p>{comments}</p>
                    </div>" : "")}
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>Next Steps:</strong></p>
                        {(actionName?.ToUpper() == "ACCEPTED" ? $@"
                        <ul>
                            <li>The engineer will contact you to schedule the visit</li>
                            <li>Confirm your availability for the scheduled date/time</li>
                            <li>Ensure someone is present at the site during the visit</li>
                            <li>You will receive a visit confirmation with exact timing</li>
                        </ul>" : $@"
                        <ul>
                            <li>The service request has been declined by the engineer</li>
                            <li>You will be contacted by the coordinator to reschedule or assign another engineer</li>
                            <li>Please contact support if you need immediate assistance</li>
                        </ul>")}
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when engineers respond to service requests.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Engineer Response - Service Request {serviceRequest.SerReqNo}: {actionName}";

                var cm = new CommonMethods(context, currentUserService, configuration);
                cm.SendEmailMethod(contact.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendCustomerEngineerActionEmailAsync] Error sending email: {ex.Message}");
            }
        }
    }
}