using Application.Features.ServiceReports;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceReports.Responses;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Domain.Entities;
using Mapster;
using Domain.Views;
using Application.Features.ServiceReports.Requests;
using Application.Models;
using System.Net.Mail;
using System.Globalization;
using Microsoft.Extensions.Configuration;

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
            serviceReportResponse.BrandId = serviceReport.BrandId;
            serviceReportResponse.BrandName = Context.Brand.FirstOrDefault(x => x.Id == serviceReport.BrandId).BrandName;
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
            return ServiceReport.Id;
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

            #region set Credential
            var client = new SmtpClient
            {
                EnableSsl = Convert.ToBoolean(appSettings.EmailSettings.SSL),
                Host = appSettings.EmailSettings.Host,
                Port = Convert.ToInt32(appSettings.EmailSettings.Port)
            };

            if (!string.IsNullOrEmpty(appSettings.EmailSettings.SMTPPassword))
            {
                client.Credentials = new System.Net.NetworkCredential(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.SMTPPassword);
                //client.UseDefaultCredentials = false;
            }
            else
                client.UseDefaultCredentials = true;
            #endregion

            client.Send(message);
            message.Dispose();
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

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


                #region set Credential
                var client = new SmtpClient
                {
                    EnableSsl = Convert.ToBoolean(_appSettings.EmailSettings.SSL),
                    Host = _appSettings.EmailSettings.Host,
                    Port = Convert.ToInt32(_appSettings.EmailSettings.Port)
                };

                if (!string.IsNullOrEmpty(_appSettings.EmailSettings.SMTPPassword))
                {
                    client.Credentials = new System.Net.NetworkCredential(_appSettings.EmailSettings.SMTPUser, _appSettings.EmailSettings.SMTPPassword);
                    //client.UseDefaultCredentials = false;
                }
                else
                    client.UseDefaultCredentials = true;
                #endregion

                await client.SendMailAsync(message);
                message.Dispose();
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            }
            catch (Exception ex)
            {                

            }
#pragma warning restore CS0168 // Variable is declared but never used
        }

    }
}
