using Application.Features.Identity.Users;
using Application.Features.ServiceReports;
using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;
using Application.Models;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Mapster;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ServiceReportService(ApplicationDbContext Context, ICurrentUserService currentUserService, IConfiguration configuration) : IServiceReportService
    {
        public async Task<Domain.Entities.ServiceReport> GetServiceReportEntityAsync(Guid id)
            => await Context.ServiceReport.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<ServiceReportResponse>> GetServiceReportsAsync()
        {
            var userProfile = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            List<ServiceReportResponse> serviceReportResponses = new();
            List<ServiceReport> serviceReports = new();
            if (userProfile != null)
            {
                if (userProfile.ContactType == "CS")
                {
                    serviceReports = await (from srp in Context.ServiceReport
                                            join sr in Context.ServiceRequest on srp.ServiceRequestId equals sr.Id
                                            where sr.CreatedBy == userProfile.UserId || sr.SiteUserId == userProfile.UserId
                                            select srp).ToListAsync();

                }
                else if (userProfile.ContactType == "DR" && userProfile.SegmentCode == "RDTSP")
                {
                    serviceReports = await (from srp in Context.ServiceReport
                                            join sr in Context.ServiceRequest on srp.ServiceRequestId equals sr.Id
                                            where sr.DistId == userProfile.EntityParentId
                                            select srp).ToListAsync();
                }
                else if (userProfile.ContactType == "DR" && userProfile.SegmentCode == "RENG")
                {
                    serviceReports = await (from srp in Context.ServiceReport
                                            join sr in Context.ServiceRequest on srp.ServiceRequestId equals sr.Id
                                            where sr.AssignedTo == userProfile.ContactId
                                            select srp).ToListAsync();
                }
            }
            else
            {
                serviceReports = await Context.ServiceReport.ToListAsync();
            }
            foreach (var report in serviceReports)
            {
                serviceReportResponses.Add(GetServiceReportResponse(report));
            }

            return serviceReportResponses;
        }

        public async Task<ServiceReportResponse> GetServiceReportAsync(Guid id)
        {
            return GetServiceReportResponse(await Context.ServiceReport.FirstOrDefaultAsync(p => p.Id == id));
        }

        public async Task<VW_ServiceReport> GetServiceReportPDFAsync(Guid id)
        {
            var attachment = Context.FileShare.Where(x => x.ParentId == id).ToList();
            var hasAttachment = false;
            if (attachment != null && attachment.Count() > 0)
            {
                hasAttachment = true;
            }
            var serviceReport = (await Context.VW_ServiceReport.FirstOrDefaultAsync(x => x.ServiceReportId == id));//.Adapt<VW_ServiceReport>();
            serviceReport.Attachment = hasAttachment;
            serviceReport.IsWorkDone = serviceReport.WorkCompleted;

            serviceReport.WorkDone = (Context.SRPEngWorkDone.Where(x => x.ServiceReportId == id).ToList());
            serviceReport.WorkTime = (Context.SRPEngWorkTime.Where(x => x.ServiceReportId == id).ToList());
            serviceReport.SPConsumed = (Context.SPConsumed.Where(x => x.ServiceReportId == id).ToList());
            serviceReport.SPRecomm = (Context.SPRecommended.Where(x => x.ServiceReportId == id).ToList());

            var wTime = serviceReport.WorkTime
                    .GroupBy(p => p.WorkTimeDate)
                    .Select(x => new TotalDays { Totaldays = x.Count() })
                    .ToList();

            serviceReport.TotalDays = wTime.Count();

            return serviceReport;
        }

        private ServiceReportResponse GetServiceReportResponse(ServiceReport serviceReport)
        {
            ServiceReportResponse serviceReportResponse = new();

            var instType = Context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == (Context.Instrument.FirstOrDefault(x => x.Id == serviceReport.InstrumentId).InsType)).ItemName;

            serviceReportResponse.AnalyticalAssit = serviceReport.AnalyticalAssit;
            serviceReportResponse.BrandId = Context.InstrumentAllocation.FirstOrDefault(x=>x.InstrumentId == serviceReport.InstrumentId).BrandId;
            serviceReportResponse.BrandName = Context.Brand.FirstOrDefault(x => x.Id == serviceReportResponse.BrandId).BrandName;
            serviceReportResponse.ComputerArlsn = serviceReport.ComputerArlsn;
            serviceReportResponse.CorrMaintenance = serviceReport.CorrMaintenance;
            serviceReportResponse.Id = serviceReport.Id;
            serviceReportResponse.Country = serviceReport.Country;
            serviceReportResponse.CreatedBy = serviceReport.CreatedBy;
            serviceReportResponse.CreatedOn = serviceReport.CreatedOn;
            serviceReportResponse.Customer = serviceReport.Customer;
            serviceReportResponse.CustSignature = serviceReport.CustSignature;
            serviceReportResponse.Department = serviceReport.Department;
            serviceReportResponse.DepartmentName = serviceReport.Department != "" ? Context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == serviceReport.Department)?.ItemName : "";
            serviceReportResponse.EngineerComments = serviceReport.EngineerComments;
            serviceReportResponse.EngSignature = serviceReport.EngSignature;
            serviceReportResponse.Firmaware = serviceReport.Firmaware;
            serviceReportResponse.Installation = serviceReport.Installation;
            serviceReportResponse.Instrument = instType + " - " + Context.Instrument.FirstOrDefault(x => x.Id == serviceReport.InstrumentId).SerialNos;
            serviceReportResponse.InstrumentId = serviceReport.InstrumentId;
            serviceReportResponse.Interrupted = serviceReport.Interrupted;
            serviceReportResponse.IsActive = serviceReport.IsActive;
            serviceReportResponse.IsCompleted = serviceReport.IsCompleted;
            serviceReportResponse.IsDeleted = serviceReport.IsDeleted;
            serviceReportResponse.LabChief = serviceReport.LabChief;
            serviceReportResponse.NextVisitScheduled = serviceReport.NextVisitScheduled;
            serviceReportResponse.PrevMaintenance = serviceReport.PrevMaintenance;
            serviceReportResponse.Problem = serviceReport.Problem;
            serviceReportResponse.Reason = serviceReport.Reason;
            serviceReportResponse.RespInstrumentId = serviceReport.RespInstrumentId;
            serviceReportResponse.RespInstrumentName = "";
            serviceReportResponse.Rework = serviceReport.Rework;
            serviceReportResponse.ServiceReportDate = serviceReport.ServiceReportDate;
            serviceReportResponse.ServiceReportNo = serviceReport.ServiceReportNo;
            serviceReportResponse.SerReqNo = Context.ServiceRequest.FirstOrDefault(x => x.Id == serviceReport.ServiceRequestId).SerReqNo;
            serviceReportResponse.ServiceRequestId = serviceReport.ServiceRequestId;
            serviceReportResponse.SignCustName = serviceReport.SignCustName;
            serviceReportResponse.SignEngName = serviceReport.SignEngName;
            serviceReportResponse.Software = serviceReport.Software;
            serviceReportResponse.SrOf = serviceReport.SrOf;
            serviceReportResponse.Town = serviceReport.Town;
            serviceReportResponse.UpdatedBy = serviceReport.UpdatedBy;
            serviceReportResponse.UpdatedOn = serviceReport.UpdatedOn;
            serviceReportResponse.WorkCompleted = serviceReport.WorkCompleted;
            serviceReportResponse.WorkFinished = serviceReport.WorkFinished;


            serviceReportResponse.WorkDone = (Context.SRPEngWorkDone.Where(x => x.ServiceReportId == serviceReport.Id).ToList()).Adapt<List<SRPEngWorkDoneResponse>>();
            serviceReportResponse.WorkTime = (Context.SRPEngWorkTime.Where(x => x.ServiceReportId == serviceReport.Id).ToList()).Adapt<List<SRPEngWorkTimeResponse>>();
            serviceReportResponse.SPConsumed = (Context.SPConsumed.Where(x => x.ServiceReportId == serviceReport.Id).ToList()).Adapt<List<SPConsumedResponse>>();
            serviceReportResponse.SPRecommended = (Context.SPRecommended.Where(x => x.ServiceReportId == serviceReport.Id).ToList()).Adapt<List<SPRecommendedResponse>>();
            serviceReportResponse.ServiceRequest = Context.ServiceRequest.FirstOrDefault(x => x.Id == serviceReport.ServiceRequestId);

            return serviceReportResponse;
        }

        public async Task<Guid> CreateServiceReportAsync(Domain.Entities.ServiceReport ServiceReport)
        {
            var repNo = "SerRpt" + DateTime.Now.Year + DateTime.Now.Month.ToString() + (Context.ServiceReport.ToList().Count + 1);

            ServiceReport.ServiceReportNo = repNo;
            ServiceReport.BrandId = Context.Instrument.FirstOrDefault(x => x.Id == ServiceReport.InstrumentId).BrandId;
            ServiceReport.ServiceReportDate = DateTime.Now;

            ServiceReport.CreatedOn = DateTime.Now;
            ServiceReport.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            ServiceReport.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            ServiceReport.UpdatedOn = DateTime.Now;

            await Context.ServiceReport.AddAsync(ServiceReport);
            await Context.SaveChangesAsync();
            return ServiceReport.Id;
        }

        public async Task<bool> DeleteServiceReportAsync(Guid id)
        {

            var deletedEngAction = await Context
                .ServiceReport.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateServiceReportAsync(Domain.Entities.ServiceReport ServiceReport)
        {
            ServiceReport.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            ServiceReport.UpdatedOn = DateTime.Now;

            Context.Entry(ServiceReport).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            // Send real-time notification to distributor when report is submitted (with engineer signature)
            if (!string.IsNullOrEmpty(ServiceReport.EngSignature))
            {
                await NotifyDistributorForServiceReportSubmissionAsync(ServiceReport);
                await NotifyDistributorForCustomerSignatureAsync(ServiceReport);
            }

            // Send real-time notification to distributor when customer signs report
            if (!string.IsNullOrEmpty(ServiceReport.CustSignature))
            {
                await NotifyDistributorForCustomerSignatureAsync(ServiceReport);
            }

            return ServiceReport.Id;
        }

        /// <summary>
        /// Notifies distributor in real-time when engineer submits service report with signature
        /// </summary>
        private async Task NotifyDistributorForServiceReportSubmissionAsync(Domain.Entities.ServiceReport serviceReport)
        {
            try
            {
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

                // Get engineer details
                var engineer = await Context.RegionContact
                    .FirstOrDefaultAsync(x => x.Id == serviceRequest.AssignedTo);

                // Check if notification already sent for this report
                var existingNotification = await Context.Notifications
                    .FirstOrDefaultAsync(x =>
                        x.Remarks.Contains($"Service Report") &&
                        x.Remarks.Contains(serviceReport.ServiceReportNo) &&
                        x.CreatedOn.Date == DateTime.Now.Date);

                if (existingNotification != null)
                {
                    return;
                }

                // Create notifications for each distributor contact
                foreach (var contact in distributorContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Service Report submitted: {serviceReport.ServiceReportNo} for Service Request: {serviceRequest.SerReqNo} at {site?.CustRegName ?? "N/A"} ({customer?.CustName ?? "N/A"}). Engineer: {engineer?.FirstName} {engineer?.LastName}",
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

                        // Send email notification
                        await SendServiceReportSubmissionEmailAsync(contact, serviceReport, serviceRequest, site, customer, engineer);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForServiceReportSubmission] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForServiceReportSubmission] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification about service report submission
        /// </summary>
        private async Task SendServiceReportSubmissionEmailAsync(
            VW_UserProfile recipient,
            Domain.Entities.ServiceReport serviceReport,
            ServiceRequest serviceRequest,
            Site site,
            Customer customer,
            RegionContact engineer)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email)) return;

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>📋 Service Report Submitted</h3>
                    
                    <div style='background-color: #d4edda; padding: 15px; border-left: 4px solid #28a745; margin: 10px 0;'>
                        <p><strong>✓ INFO:</strong> Engineer has submitted service report for your review.</p>
                    </div>
                    
                    <h4>Service Report Details:</h4>
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
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Engineer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{engineer?.FirstName} {engineer?.LastName}</td>
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
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportDate:dd/MM/yyyy}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Report Status:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #28a745;'>{(serviceReport.IsCompleted ? "COMPLETED" : "SUBMITTED")}</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Work Finished:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{(serviceReport.WorkFinished ? "Yes" : "No")}</td>
                        </tr>
                    </table>
                    
                    {(string.IsNullOrEmpty(serviceReport.EngineerComments) ? "" : $@"
                    <h4>Engineer Comments:</h4>
                    <div style='padding: 15px; background-color: #f0f0f0; border-left: 4px solid #007bff; margin-bottom: 20px;'>
                        <p>{serviceReport.EngineerComments}</p>
                    </div>")}
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>✓ Next Steps:</strong></p>
                        <ul>
                            <li>Review the submitted service report in the CIM system</li>
                            <li>Verify all service details are complete and accurate</li>
                            <li>Approve the service report if satisfactory</li>
                            <li>Contact engineer if any clarification is needed</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Service Summary:</strong></p>
                        <ul>
                            <li>Installation Service: {(serviceReport.Installation ? "Yes" : "No")}</li>
                            <li>Preventive Maintenance: {(serviceReport.PrevMaintenance ? "Yes" : "No")}</li>
                            <li>Corrective Maintenance: {(serviceReport.CorrMaintenance ? "Yes" : "No")}</li>
                            <li>Work Completed: {(serviceReport.WorkCompleted ? "Yes" : "No")}</li>
                            {(serviceReport.Interrupted ? $"<li style='color: #dc3545;'><strong>Service Interrupted: Yes</strong> - {serviceReport.Reason ?? "No reason provided"}</li>" : "")}
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when service reports are submitted.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Service Report {serviceReport.ServiceReportNo} Submitted - SR: {serviceRequest.SerReqNo}";

                var cm = new CommonMethods(Context, currentUserService, configuration);
                cm.SendEmailMethod(recipient.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendServiceReportSubmissionEmailAsync] Error sending email: {ex.Message}");
            }
        }

        public async Task<bool> UploadServiceReportAsync(UploadServiceReportRequest uploadServiceReport)
        {
            var serreq = await Context.ServiceRequest.FirstOrDefaultAsync(x => x.Id == uploadServiceReport.SerReqId);
            var serRep = await Context.ServiceReport.FirstOrDefaultAsync(x => x.ServiceRequestId == uploadServiceReport.SerReqId);

            serreq.IsReportGenerated = true;
            serreq.SerResolutionDate = DateTime.Today.ToString("dd/MM/yyy");
            serreq.StageId = Context.VW_ListItems.FirstOrDefault(x => x.ListCode == "SRSAT" && x.ItemCode == "COMP").ListTypeItemId;
            Context.Entry(serreq).State = EntityState.Modified;

            serRep.IsCompleted = true;
            Context.Entry(serRep).State = EntityState.Modified;
            Context.SaveChanges();

            UpdateAMC(serreq);

            var engCon = Context.RegionContact.FirstOrDefault(x => x.Id == serreq.AssignedTo);
            var EngName = engCon.FirstName + " " + engCon.LastName;

            var Attachment = SaveAttachments(uploadServiceReport.Pdf, uploadServiceReport.SerReqId.ToString(), serreq.SerReqNo);

            //SRAuditTrailBO Obj = new SRAuditTrailBO(Context);
            //Obj.SaveSRAudit("Engineer Action", EngName + " has generated the Service report and email is sent to respective stakeholders.", uploadServiceReport.SerReqId, userId, serRep.CompanyId);


            var message = new MailMessage();
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            #region set To,Cc abd Bcc
            message.To.Add(new MailAddress(serreq?.Email));
            message.To.Add(new MailAddress(serreq?.OperatorEmail));
            message.CC.Add(new MailAddress("kishoregund@gmail.com"));
            var demails = appSettings.DistEmails.Split(',');
            foreach (string email in demails)
            {
                message.CC.Add(new MailAddress(email));
            }

            //message.Bcc.Add(new MailAddress(arrStr.Trim()));
            //message.ReplyToList.Add(new MailAddress(arrStr.Trim(), "reply-to"));
            #endregion

            #region set Email body
            var body = $"Dear {serreq?.CompanyName},<br> Please find attached the service report for Service Request No. {serreq?.SerReqNo} Generated by {EngName} <br> Thanks,<br> Avante Garde<br> <br> *This is a system generated email.Please do not reply.";
            message.From = new MailAddress(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.DisplayName);
            message.Subject = serreq.SerReqNo;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.Body = body;

            if (Attachment != null)
            {
                message.Attachments.Add(new Attachment(Attachment));
            }
            #endregion

            using (var client = new SmtpClient())
            {
                // Office 365 SMTP Settings
                client.Host = appSettings.EmailSettings.Host;
                client.Port = Convert.ToInt32(appSettings.EmailSettings.Port);
                client.EnableSsl = Convert.ToBoolean(appSettings.EmailSettings.SSL);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // Use YOUR EMAIL and APP-SPECIFIC PASSWORD
                // NOT your regular password
                client.Credentials = new NetworkCredential(
                    appSettings.EmailSettings.SMTPUser,
                    appSettings.EmailSettings.SMTPPassword  // Generated from Microsoft Account
                );

                // Set timeout to avoid connection issues
                client.Timeout = 60000; // 60 seconds

                client.Send(message);
            }

            if (Attachment != null)
            {
                File.Delete(Attachment);
            }

            #region sparepart recommended email
            try
            {
                SendEmail(serreq, serRep, appSettings, EngName);
            }
            catch (Exception ex)
            { }

            #endregion

            return true;

        }

        private void UpdateAMC(ServiceRequest mServiceRequest)
        {
            AMC insUnderAMC = new AMC();
            List<AMC> lstAMCs = (from amcInstrument in Context.AMCInstrument.Where(x => x.InstrumentId.ToString() == mServiceRequest.MachinesNo)
                                 join amc in Context.AMC.Where(x => x.IsActive)
                                 on amcInstrument.AMCId equals amc.Id
                                 select amc).ToList();
            foreach (AMC a in lstAMCs)
            {

                if (DateTime.ParseExact(mServiceRequest.SerReqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= DateTime.ParseExact(a.SDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date
                    && DateTime.ParseExact(mServiceRequest.SerReqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= DateTime.ParseExact(a.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date)
                {
                    insUnderAMC = a;
                }
            }

            AMCItems aItem = new AMCItems();

            var lstAItem = Context.AMCItems.Where(x => x.AMCId == insUnderAMC.Id && !x.IsDeleted && x.ServiceType == mServiceRequest.VisitType).ToList();
            var breakDownId = Context.VW_ListItems.Where(x => x.ListCode == "SERTY" && x.ItemCode == "BRKDW").FirstOrDefault().ListTypeItemId;
            foreach (AMCItems a in lstAItem)
            {
                if (insUnderAMC.IsMultipleBreakdown && a.ServiceType == breakDownId.ToString())
                {
                    a.ServiceRequestId = (a.ServiceRequestId + "," + mServiceRequest.Id);
                    a.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    a.EstStartDate = mServiceRequest.SerReqDate;
                    a.Status = Context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "AICON" && x.ListCode == "AISTA").ListTypeItemId.ToString();

                    Context.Entry(a).State = EntityState.Modified;
                    Context.SaveChanges();
                    break;
                }
                else if (string.IsNullOrEmpty(a.ServiceRequestId))
                {
                    a.ServiceRequestId = mServiceRequest.Id.ToString();
                    a.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    a.EstStartDate = mServiceRequest.SerReqDate;
                    a.Status = Context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "AICON" && x.ListCode == "AISTA").ListTypeItemId.ToString();

                    Context.Entry(a).State = EntityState.Modified;
                    Context.SaveChanges();
                    break;
                }
            }

            //if (insUnderAMC != null && !insUnderAMC.Ismultiplebreakdown)
            //{
            //var lstAmcObj = (from amcInstrument in
            //        Context.AmcInstrument.Where(x => x.InstrumentId == mServiceRequest.Machinesno)
            //                 join amc in Context.AMC.Where(x => x.Isactive) on amcInstrument.AmcId equals amc.Id
            //                 join amcItems in Context.AmcItem.Where(x =>
            //                         !x.IsDeleted && string.IsNullOrEmpty(x.ServiceRequestId))
            //                     on amc.Id equals amcItems.AmcId   
            //                 select new { amc, amcItems }).ToList();

            //insUnderAMC = lstAmcObj.Find(x => x.amcItems.ServiceType == mServiceRequest.Visittype)?.amcItems;

            //}


            //if (insUnderAMC != null)
            //{
                // only service request should be updated with service quote no and not the amc items

                //insUnderAMC.amcItems.ServiceRequestId = insUnderAMC.amc.Ismultiplebreakdown ? (insUnderAMC.amcItems.ServiceRequestId + "," + mServiceRequest.Id) : mServiceRequest.Id;
                //insUnderAMC.amcItems.Date = DateTime.Now.ToString("dd/MM/yyyy");
                //insUnderAMC.amcItems.EstStartDate = mServiceRequest.Serreqdate;
                //insUnderAMC.amcItems.Status = Context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "AICON" && x.ListCode == "AISTA")?.ListTypeItemId;

                //Context.Entry(insUnderAMC.amcItems).State = EntityState.Modified;
                //Context.SaveChanges();

            //}
        }

        private string SaveAttachments(string data, string id, string srreqno)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var bytes = Convert.FromBase64String(data);
                var folderName = Path.Combine("FilesShare", "Attachments");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                pathToSave = pathToSave + $"\\SR_{srreqno}.pdf";
                var stream = new FileStream(pathToSave, FileMode.CreateNew);
                var writer = new BinaryWriter(stream);
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();

                return pathToSave;
            }
            catch (Exception ex)
            {
                return null;
            }
#pragma warning restore CS0168 // Variable is declared but never used

        }

        private async void SendEmail(ServiceRequest serRequest, ServiceReport serReport, AppSettings _appSettings,string engName)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {

                //var sprObj = new SparePartsRecommendedDL(_context);
                //var serReqObj = new ServiceRequestDL(_context);
                //var serReq = serReqObj.GetServiceRequestById(serReqId, userId, bUId, brandId);
                var spRecomm = Context.SPRecommended.Where(x => x.ServiceReportId == serReport.Id).ToList();  //sprObj.GetSparesRecommendedBySRPId(serRepId, userId, bUId, brandId);

                var mailBody = $"Hi {serRequest.OperatorName}, <br>  <br> The engineer {engName} has recommended below spare parts for the " +
                    $"<b>Service Request No.</b>:{serRequest.SerReqNo}<br><br>";

                var message = new MailMessage();

                #region set To,Cc abd Bcc

                message.To.Add(new MailAddress(serRequest.Email));
                message.To.Add(new MailAddress(serRequest.OperatorEmail));
                message.CC.Add(new MailAddress("kishoregund@gmail.com"));
                var demails = _appSettings.DistEmails.Split(',');
                foreach (string email in demails)
                {
                    message.CC.Add(new MailAddress(email));
                }
                //message.CC.Add(new MailAddress(arrStr.Trim()));
                //message.Bcc.Add(new MailAddress(arrStr.Trim()));
                //message.ReplyToList.Add(new MailAddress(arrStr.Trim(), "reply-to"));
                #endregion


                #region set Email body
                var body = "";
                if (spRecomm.Count > 0 && spRecomm != null)
                {
                    body = body + " <table border='1'><thead><tr><th> Part No.</th><th> Qty.</th><th> HSC Code </th></tr></thead><tbody>";
                    foreach (var spr in spRecomm)
                    {
                        body = body + $"<tr><td>{spr.PartNo}</td> <td>{spr.QtyRecommended}</td> <td>{spr.HscCode}</td></tr>";
                    }
                }
                body = body + "</tbody></table> <br><br><br> Thanks, <br> Avantgarde Support Team";
                body = body + "<br><br><br><br> *This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator";

                message.From = new MailAddress(_appSettings.EmailSettings.SMTPUser, _appSettings.EmailSettings.DisplayName);
                message.Subject = serRequest.SerReqNo;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;

                message.Body = mailBody + body;
                #endregion


                using (var client = new SmtpClient())
                {
                    // Office 365 SMTP Settings
                    client.Host = _appSettings.EmailSettings.Host;
                    client.Port = Convert.ToInt32(_appSettings.EmailSettings.Port);
                    client.EnableSsl = Convert.ToBoolean(_appSettings.EmailSettings.SSL);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    // Use YOUR EMAIL and APP-SPECIFIC PASSWORD
                    // NOT your regular password
                    client.Credentials = new NetworkCredential(
                        _appSettings.EmailSettings.SMTPUser,
                        _appSettings.EmailSettings.SMTPPassword  // Generated from Microsoft Account
                    );

                    // Set timeout to avoid connection issues
                    client.Timeout = 60000; // 60 seconds

                    client.Send(message);
                }

            }
            catch (Exception ex)
            {                

            }
#pragma warning restore CS0168 // Variable is declared but never used
        }

        /// <summary>
        /// Notifies distributor in real-time when customer signs service report
        /// </summary>
        private async Task NotifyDistributorForCustomerSignatureAsync(Domain.Entities.ServiceReport serviceReport)
        {
            try
            {
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

                // Get distributor contacts (RDTSP - Regional Distributors) & engineers for customer signature notification
                var distributorContacts = await Context.VW_UserProfile
                    .Where(x => (x.SegmentCode == "RDTSP" || x.SegmentCode == "RENG") && x.EntityParentId == distributor.Id)
                    .ToListAsync();

                if (distributorContacts.Count == 0) return;

                // Get engineer details
                var engineer = await Context.RegionContact
                    .FirstOrDefaultAsync(x => x.Id == serviceRequest.AssignedTo);

                //// Check if notification already sent for this report
                //var existingNotification = await Context.Notifications
                //    .FirstOrDefaultAsync(x =>
                //        x.Remarks.Contains($"Customer signed") &&
                //        x.Remarks.Contains(serviceReport.ServiceReportNo) &&
                //        x.CreatedOn.Date == DateTime.Now.Date);

                //if (existingNotification != null)
                //{
                //    return;
                //}

                // Create notifications for each distributor contact
                foreach (var contact in distributorContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Service Report signed by customer: {serviceReport.ServiceReportNo} for Service Request: {serviceRequest.SerReqNo} at {site?.CustRegName ?? "N/A"} ({customer?.CustName ?? "N/A"}). Signed by: {serviceReport.SignCustName}. Engineer: {engineer?.FirstName} {engineer?.LastName}",
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

                        // Send email notification
                        await SendCustomerSignatureEmailAsync(contact, serviceReport, serviceRequest, site, customer, engineer);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForCustomerSignature] Error notifying contact {contact.UserId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForCustomerSignature] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification about customer signature on service report
        /// </summary>
        private async Task SendCustomerSignatureEmailAsync(
            VW_UserProfile recipient,
            Domain.Entities.ServiceReport serviceReport,
            ServiceRequest serviceRequest,
            Site site,
            Customer customer,
            RegionContact engineer)
        {
            try
            {
                if (string.IsNullOrEmpty(recipient.Email)) return;

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>✅ Service Report Signed by Customer</h3>
                    
                    <div style='background-color: #d4edda; padding: 15px; border-left: 4px solid #28a745; margin: 10px 0;'>
                        <p><strong>✓ CONFIRMED:</strong> Customer has signed the service report. Service completion approved by customer.</p>
                    </div>
                    
                    <h4>Service Report Details:</h4>
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
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Engineer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{engineer?.FirstName} {engineer?.LastName}</td>
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
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceReport.ServiceReportDate:dd/MM/yyyy}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Signed By:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #28a745;'>{serviceReport.SignCustName}</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Report Status:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #28a745;'>{(serviceReport.IsCompleted ? "COMPLETED & APPROVED" : "SIGNED")}</strong></td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Work Finished:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{(serviceReport.WorkFinished ? "Yes" : "No")}</td>
                        </tr>
                    </table>
                    
                    {(string.IsNullOrEmpty(serviceReport.EngineerComments) ? "" : $@"
                    <h4>Engineer Comments:</h4>
                    <div style='padding: 15px; background-color: #f0f0f0; border-left: 4px solid #007bff; margin-bottom: 20px;'>
                        <p>{serviceReport.EngineerComments}</p>
                    </div>")}
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #d4edda; border-left: 4px solid #28a745;'>
                        <p><strong>✓ Customer Approval Confirmed:</strong></p>
                        <ul>
                            <li>✓ Service work has been completed and approved by customer</li>
                            <li>✓ Customer has signed the service report</li>
                            <li>✓ Service is now documented and finalized</li>
                            <li>✓ Report ready for billing/invoicing</li>
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #f0f0f0;'>
                        <p><strong>Service Summary:</strong></p>
                        <ul>
                            <li>Installation Service: {(serviceReport.Installation ? "Yes" : "No")}</li>
                            <li>Preventive Maintenance: {(serviceReport.PrevMaintenance ? "Yes" : "No")}</li>
                            <li>Corrective Maintenance: {(serviceReport.CorrMaintenance ? "Yes" : "No")}</li>
                            <li>Work Completed: {(serviceReport.WorkCompleted ? "Yes" : "No")}</li>
                            {(serviceReport.Interrupted ? $"<li style='color: #dc3545;'><strong>Service Interrupted: Yes</strong> - {serviceReport.Reason ?? "No reason provided"}</li>" : "")}
                        </ul>
                    </div>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>Next Steps:</strong></p>
                        <ul>
                            <li>Review the signed service report</li>
                            <li>Verify all service details and signatures</li>
                            <li>Process billing/invoicing if applicable</li>
                            <li>Archive the signed report</li>
                            <li>Close the service request if all work is complete</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when service reports are signed by customers.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"✓ Service Report {serviceReport.ServiceReportNo} - SIGNED BY CUSTOMER - SR: {serviceRequest.SerReqNo}";

                var cm = new CommonMethods(Context, currentUserService, configuration);
                cm.SendEmailMethod(recipient.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendCustomerSignatureEmailAsync] Error sending email: {ex.Message}");
            }
        }
    }
}
