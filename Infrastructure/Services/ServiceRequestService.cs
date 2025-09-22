using Application.Features.ServiceRequests;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Application.Features.ServiceRequests.Responses;
using Application.Features.ServiceRequests.Queries;
using Application.Features.Customers.Responses;
using Mapster;
using Infrastructure.Common;
using System.ComponentModel.Design;
using System.Globalization;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Domain.Views;

namespace Infrastructure.Services
{
    public class ServiceRequestService(ApplicationDbContext context, ICurrentUserService currentUserService,
        IEngSchedulerService engSchedulerService = null, ISRAssignedHistoryService srAssignedHistoryService = null, IConfiguration  configuration =null,
        ISREngActionService srEngActionService = null, ISREngCommentsService srEngCommentsService = null) : IServiceRequestService
    {

        public async Task<ServiceRequestResponse> GetServiceRequestAsync(Guid id)
        {
            var sr = await context.ServiceRequest.FirstOrDefaultAsync(p => p.Id == id);
            return GetServiceRequestDetail(sr);
        }
        public async Task<ServiceRequest> GetServiceRequestEntityAsync(Guid id)
            => await context.ServiceRequest.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Domain.Entities.ServiceRequest>> GetServiceRequestsAsync()
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            if (userProfile != null)
            {
                if (userProfile.SegmentCode == "RENG")
                {
                    return await context.ServiceRequest.Where(x => x.DistId == userProfile.EntityParentId && x.AssignedTo.ToString() == currentUserService.GetUserId()).ToListAsync();
                }
                else if (userProfile.SegmentCode == "RDTSP")
                {
                    return await context.ServiceRequest.Where(x => x.DistId == userProfile.EntityParentId).ToListAsync();
                }
                //else if (userProfile.SegmentCode == "RCUST")
                //{
                    return await context.ServiceRequest.Where(x => x.CreatedBy == userProfile.UserId).ToListAsync();
                //}
            }
            else
            {
                return await context.ServiceRequest.ToListAsync();
            }
        }

        public async Task<List<ServiceRequestResponse>> GetDetailServiceRequestsAsync(string businessUnitId, string brandId)
        {
            List<ServiceRequestResponse> serviceRequestResponses = new();
            List<ServiceRequest> serviceRequests = new();
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));


                if (context.Users.FirstOrDefault(x => x.Id == currentUserService.GetUserId()).FirstName != "Admin")
                {
                    if (userProfile != null)
                    {
                        var segment = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == userProfile.SegmentId).ItemCode;
                        if (segment.ToUpper().Equals("RDTSP") && userProfile.ContactType == "DR")
                        {
                            serviceRequests = await (from sr in context.ServiceRequest.Where(x => !x.IsDeleted)
                                                     join i in context.Instrument.Where(x => businessUnitId.ToString().Contains(x.BusinessUnitId.ToString()) && brandId.ToString().Contains(x.BrandId.ToString()))
                                                         on sr.MachinesNo equals i.Id.ToString()
                                                     select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                            // first pullout SR for the loggedin user Distributor
                            var distRegions = userProfile.DistRegions.Split(',');
                            serviceRequests = serviceRequests.Where(x => x.DistId == userProfile.EntityParentId)
                                .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();

                            //find customers for defaultregions of the loggedin users distregions
                            var custSites = (from c in context.Customer //.Where(x => distRegions.Contains(x.DefDistRegionId.ToString()))
                                             join s in context.Site.Where(x => distRegions.Contains(x.RegionId.ToString())) on c.Id equals s.CustomerId
                                             select s.Id).ToList();

                            // get SR for those sites for whom permissible distregions are assigned
                            serviceRequests = serviceRequests.Where(x => custSites.Contains(x.SiteId) && x.DistId == userProfile.EntityParentId)
                                .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                        }
                        else if (segment.ToUpper().Equals("RCUST") && userProfile.ContactType == "CS")
                        {
                            serviceRequests = await context.ServiceRequest.ToListAsync();

                            //serviceRequests = await (from sr in context.ServiceRequest
                            //                         // join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                            //                         select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                            var custSites = userProfile.CustSites.Split(',');
                            serviceRequests = serviceRequests.Where(x => custSites.Contains(x.SiteId.ToString()) && x.CustId == userProfile.EntityParentId)
                                .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                        }
                        else if (segment.ToUpper().Equals("RENG"))
                        {
                            serviceRequests = await (from sr in context.ServiceRequest.Where(x => !x.IsDeleted)
                                                     //join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                                                     select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                            serviceRequests = serviceRequests.Where(x => x.AssignedTo == userProfile.ContactId)
                                .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                        }
                    }
                }

                foreach (ServiceRequest sr in serviceRequests)
                {
                    serviceRequestResponses.Add(GetServiceRequestDetail(sr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceRequestResponses;
        }

        public async Task<List<ServiceRequest>> GetDetailServiceRequestsOnlyAsync(string businessUnitId, string brandId)
        {
            List<ServiceRequestResponse> serviceRequestResponses = new();
            List<ServiceRequest> serviceRequests = new();

            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));


            if (context.Users.FirstOrDefault(x => x.Id == currentUserService.GetUserId()).FirstName != "Admin")
            {
                if (userProfile != null)
                {
                    var segment = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == userProfile.SegmentId).ItemCode;
                    if (segment.ToUpper().Equals("RDTSP") && userProfile.ContactType == "DR")
                    {
                        serviceRequests = await (from sr in context.ServiceRequest.Where(x => !x.IsDeleted)
                                                 join i in context.Instrument.Where(x => businessUnitId.ToString().Contains(x.BusinessUnitId.ToString()) && brandId.ToString().Contains(x.BrandId.ToString()))
                                                     on sr.MachinesNo equals i.Id.ToString()
                                                 select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                        // first pullout SR for the loggedin user Distributor
                        var distRegions = userProfile.DistRegions.Split(',');
                        serviceRequests = serviceRequests.Where(x => x.DistId == userProfile.EntityParentId)
                            .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();

                        //find customers for defaultregions of the loggedin users distregions
                        var custSites = (from c in context.Customer.Where(x => distRegions.Contains(x.DefDistRegionId.ToString()))
                                         join s in context.Site on c.Id equals s.CustomerId
                                         select s.Id).ToList();

                        // get SR for those sites for whom permissible distregions are assigned
                        serviceRequests = serviceRequests.Where(x => custSites.Contains(x.SiteId) && x.DistId == userProfile.EntityParentId)
                            .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                    }
                    else if (segment.ToUpper().Equals("RCUST") && userProfile.ContactType == "CS")
                    {
                        serviceRequests = await (from sr in context.ServiceRequest.Where(x => !x.IsDeleted)
                                                 join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                                                 select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                        var custSites = userProfile.CustSites.Split(',');
                        serviceRequests = serviceRequests.Where(x => custSites.Contains(x.SiteId.ToString()) && x.CustId == userProfile.EntityParentId)
                            .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                    }
                    else if (segment.ToUpper().Equals("RENG"))
                    {
                        serviceRequests = await (from sr in context.ServiceRequest.Where(x => !x.IsDeleted)
                                                 join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                                                 select sr).OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToListAsync();

                        serviceRequests = serviceRequests.Where(x => x.AssignedTo == userProfile.ContactId)
                            .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
                    }
                }
            }

            //foreach (ServiceRequest sr in serviceRequests)
            //{
            //    serviceRequestResponses.Add(GetServiceRequestDetail(sr));
            //}
            return serviceRequests;
        }

        public async Task<Guid> CreateServiceRequestAsync(Domain.Entities.ServiceRequest serviceRequest)
        {
            serviceRequest.SerReqNo = await GetServiceRequestNoAsync();
            serviceRequest.AlarmDetails = string.IsNullOrEmpty(serviceRequest.AlarmDetails) ? "Breakdown" : serviceRequest.AlarmDetails;
            serviceRequest.BreakdownType = string.IsNullOrEmpty(serviceRequest.BreakdownType) ? "Breakdown" : serviceRequest.BreakdownType;
            serviceRequest.BreakoccurDetailsId = serviceRequest.BreakoccurDetailsId == Guid.Empty ? context.VW_ListItems.
                        Where(x => x.ListCode == "BDOD" && x.ItemName.ToUpper() == "BREAKDOWN").FirstOrDefault().ListTypeItemId : serviceRequest.BreakoccurDetailsId;
            serviceRequest.CurrentInstrustatus = string.IsNullOrEmpty(serviceRequest.CurrentInstrustatus) ? context.VW_ListItems.
                Where(x => x.ListCode == "CINSS" && x.ItemName.ToUpper() == "BREAKDOWN").FirstOrDefault().ListTypeItemId.ToString() : serviceRequest.CurrentInstrustatus;
            serviceRequest.StatusId = (serviceRequest.StatusId == Guid.Empty) ? context.VW_ListItems.
                Where(x => x.ListCode == "SRQST" && x.ItemCode.ToUpper() == "NTASS").FirstOrDefault().ListTypeItemId : serviceRequest.StatusId;
            serviceRequest.StageId = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "SRSAT" && x.ItemCode == "NTSTD").ListTypeItemId;

            serviceRequest.CreatedOn = DateTime.Now;
            serviceRequest.UpdatedOn = DateTime.Now;
            serviceRequest.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            serviceRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            if (serviceRequest.TotalCost != null && serviceRequest.BaseAmt != null)
            {
                serviceRequest.CostInUsd = serviceRequest.TotalCost.Value * serviceRequest.BaseAmt.Value;
            }

            //using (var transaction = new TransactionScope())
            //{
            AMC insUnderAMC = new AMC();
            List<AMC> lstAMCs = (from amcInstrument in context.AMCInstrument.Where(x => x.InstrumentId.ToString() == serviceRequest.MachinesNo)
                                 join amc in context.AMC.Where(x => x.IsActive)
                                 on amcInstrument.AMCId equals amc.Id
                                 select amc).ToList();
            foreach (AMC a in lstAMCs)
            {
                if (DateTime.ParseExact(serviceRequest.SerReqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= DateTime.ParseExact(a.SDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date
                    && DateTime.ParseExact(serviceRequest.SerReqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= DateTime.ParseExact(a.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date)
                {
                    insUnderAMC = a;
                    break;
                }
            }

            if (insUnderAMC != null && !insUnderAMC.IsMultipleBreakdown)
            {
                if (!context.AMCItems.Any(x => x.AMCId == insUnderAMC.Id && !x.IsDeleted && x.ServiceType == serviceRequest.VisitType
                 && string.IsNullOrEmpty(x.ServiceRequestId)))
                {
                    insUnderAMC = null;
                }
            }

            if (insUnderAMC != null && InsAMCContractStage(serviceRequest))
            {
                serviceRequest.AmcServiceQuote = insUnderAMC.ServiceQuote;
                serviceRequest.AmcId = insUnderAMC.Id;
            }
            else
            {
                serviceRequest.StageId = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "SRSAT" && x.ItemCode == "ONHLD").ListTypeItemId;
                serviceRequest.IsNotUnderAmc = true;

                //result.Result = true;
                //result.ResultMessage = "Please verify if the AMC is defined and a Contract Agreement is done in AMC to benefit AMC services.";
                
            }

            await context.ServiceRequest.AddAsync(serviceRequest);
            await context.SaveChangesAsync();

            var regionId = context.Site.FirstOrDefault(x => x.Id == serviceRequest.SiteId)?.DistId;
            var lstEmails = (from users in context.VW_UserProfile.Where(x => x.SegmentCode == "RDTSP" && x.EntityChildId == regionId)
                             select users.Email).ToList();
            
            var email = lstEmails.Aggregate("", (current, t) => current + (t + ","));
            if (email.Length > 1) email = email.Remove(email.Length - 1, 1);

            CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);
            if (serviceRequest.IsNotUnderAmc)
            {
                var body = $"Hi,<br><br> Greetings! <br><br>Service Request having Service Request No.: {serviceRequest.SerReqNo} has been raised by Customer and it is not covered in the AMC or the Contract Agreement is not done. <br>"
                    + "Kindly look into it and update at the earliest. " 
                    + "<br><br><br>Thank you,<br>Avante Team<br>";
                body += "<br><br><br><br> *This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator";
                const string subject = "Uncovered Service Request";
                
                commonMethods.SendEmailMethod(email, body, subject);
            }
            else
            {
                var body = $"Hi,<br><br> Greetings! <br><br>Service Request having Service Request No.: {serviceRequest.SerReqNo} has been raised by Customer for resolution. <br>"
                    + "Kindly look into it and update at the earliest. "
                    + "<br><br><br>Thank you,<br>Avante Team<br>";
                body += "<br><br><br><br> *This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator";
                const string subject = "Service Request Raised";

                commonMethods.SendEmailMethod(email, body, subject);
            }

            //    transaction.Complete();
            //}

            #region Notification

            //var userProfile = _context.VW_UserProfiles.FirstOrDefault(x => x.Userid == mServiceRequest.Createdby);
            //var userRole = _context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == userProfile.Roleid);

            //// if distributor raised the request dont show notification about that request
            //if (userRole?.ItemCode?.ToUpper() != "RDTSP")
            //{
            //    var notification = new NotificationDL(_context);
            //    notification.MapUserToNotification(userId,
            //        $"New Service Request {mServiceRequest.Serreqno} has been Created", companyId,
            //        mServiceRequest.Createdby,
            //        null,
            //        mServiceRequest.Assignedto);
            //}

            #endregion

            return serviceRequest.Id;
        }

        private bool InsAMCContractStage(ServiceRequest serviceRequest)
        {

            var lstAmcObj = (from amcInstrument in
                      context.AMCInstrument.Where(x => x.InstrumentId.ToString() == serviceRequest.MachinesNo)
                             join amc in context.AMC.Where(x => x.IsActive) on amcInstrument.AMCId equals amc.Id
                             join amcstages in context.AMCStages.Where(x => !x.IsDeleted) on amc.Id equals amcstages.AMCId
                             select new { amc, amcstages }).ToList();

            var insUnderAMC = lstAmcObj.Find(x => x.amcstages.Stage == context.VW_ListItems.FirstOrDefault(x => x.ListCode == "AMCSG" && x.ItemCode == "CONAG").ListTypeItemId.ToString());
            if (insUnderAMC != null)
                return true;
            else
                return false;
        }


        public async Task<bool> OnBehalfOfCheck(Guid createdBy)
           => await context.VW_UserProfile.AnyAsync(x => x.UserId == createdBy && x.ContactType == "DR");

        public async Task<bool> DeleteServiceRequestAsync(Guid id)
        {

            var deletedEngAction = await context
                .ServiceRequest.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            context.Entry(deletedEngAction).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateServiceRequestAsync(Domain.Entities.ServiceRequest serviceRequest)
        {
            serviceRequest.UpdatedOn = DateTime.Now;
            serviceRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(serviceRequest).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return serviceRequest.Id;
        }

        private ServiceRequestResponse GetServiceRequestDetail(ServiceRequest ServiceRequest)
        {
            var stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.StageId);
            var mServiceRequest = new ServiceRequestResponse();
            mServiceRequest.Id = ServiceRequest.Id;
            mServiceRequest.CreatedOn = ServiceRequest.CreatedOn;
            mServiceRequest.SerReqNo = ServiceRequest.SerReqNo;
            mServiceRequest.Accepted = false;
            mServiceRequest.Escalation = ServiceRequest.Escalation;
            mServiceRequest.IsReportGenerated = ServiceRequest.IsReportGenerated;
            mServiceRequest.AcceptedDate = ServiceRequest.AcceptedDate;
            mServiceRequest.AlarmDetails = ServiceRequest.AlarmDetails;
            mServiceRequest.AssignedTo = ServiceRequest.AssignedTo;
            if (ServiceRequest.AssignedTo != Guid.Empty)
            {
                var assignTo = context.RegionContact.FirstOrDefault(x => x.Id == ServiceRequest.AssignedTo);
                mServiceRequest.AssignedToName = assignTo != null ? assignTo.FirstName + ' ' + assignTo.LastName : "";
            }
            mServiceRequest.BreakdownType = ServiceRequest.BreakdownType;
            mServiceRequest.BreakoccurDetailsId = ServiceRequest.BreakoccurDetailsId;
            mServiceRequest.BreakoccurDetails = context.VW_ListItems.Where(x => x.ListTypeItemId == ServiceRequest.BreakoccurDetailsId)?.FirstOrDefault()?.ItemName;
            mServiceRequest.ComplaintRegisName = ServiceRequest.ComplaintRegisName;
            mServiceRequest.ContactPerson = ServiceRequest.ContactPerson;
            mServiceRequest.Country = ServiceRequest.Country;
            mServiceRequest.CountryName = context.Country.FirstOrDefault(x => x.Id.ToString() == ServiceRequest.Country)?.Name;
            mServiceRequest.CurrentInstruStatus = ServiceRequest.CurrentInstrustatus;
            mServiceRequest.CurrentInstruStatusName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == ServiceRequest.CurrentInstrustatus)?.ItemName;
            mServiceRequest.Distributor = context.Distributor.Where(x => x.Id == ServiceRequest.DistId)?.FirstOrDefault()?.DistName;
            mServiceRequest.Email = ServiceRequest.Email;
            mServiceRequest.IsActive = ServiceRequest.IsActive;
            mServiceRequest.IsRecurring = ServiceRequest.IsRecurring;
            mServiceRequest.MachinesNo = ServiceRequest.MachinesNo;
            mServiceRequest.MachmodelName = ServiceRequest.MachmodelName;
            //mServiceRequest.MachmodelNameText = context.VW_ListItems.Where(x => x.ListTypeItemId.ToString() == ServiceRequest.MachmodelName)?.FirstOrDefault()?.ItemName;
            mServiceRequest.MachEngineer = ServiceRequest.MachEngineer;
            mServiceRequest.OperatorEmail = ServiceRequest.OperatorEmail;
            mServiceRequest.OperatorName = ServiceRequest.OperatorName;
            mServiceRequest.OperatorNumber = ServiceRequest.OperatorNumber;
            mServiceRequest.RecurringComments = ServiceRequest.RecurringComments;
            mServiceRequest.RegistrarPhone = ServiceRequest.RegistrarPhone;
            mServiceRequest.RequestTime = ServiceRequest.RequestTime;
            mServiceRequest.ResolveAction = ServiceRequest.ResolveAction;
            mServiceRequest.SampleHandlingType = ServiceRequest.SampleHandlingType;
            mServiceRequest.SerReqDate = ServiceRequest.SerReqDate;
            mServiceRequest.SiteName = context.Site.FirstOrDefault(x => x.Id == ServiceRequest.SiteId)?.CustRegName;
            mServiceRequest.VisitType = ServiceRequest.VisitType;
            mServiceRequest.VisitTypeName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == ServiceRequest.VisitType)?.ItemName;
            mServiceRequest.XrayGenerator = ServiceRequest.XrayGenerator;
            mServiceRequest.SiteId = ServiceRequest.SiteId;
            mServiceRequest.CustId = ServiceRequest.CustId;
            mServiceRequest.DistId = ServiceRequest.DistId;
            mServiceRequest.RequestTypeId = ServiceRequest.RequestTypeId;
            mServiceRequest.RequestType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == ServiceRequest.RequestTypeId)?.ItemName;
            mServiceRequest.SubRequestTypeId = ServiceRequest.SubRequestTypeId;
            mServiceRequest.StatusId = ServiceRequest.StatusId;
            mServiceRequest.StatusName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.StatusId)?.ItemName;
            mServiceRequest.Remarks = ServiceRequest.Remarks;
            mServiceRequest.SerResolutionDate = ServiceRequest.SerResolutionDate;
            mServiceRequest.SDate = ServiceRequest.SDate;
            mServiceRequest.EDate = ServiceRequest.EDate;
            mServiceRequest.MachinesModelName = context.Instrument.Where(x => x.Id.ToString() == ServiceRequest.MachinesNo)?.FirstOrDefault()?.SerialNos;
            mServiceRequest.StageId = ServiceRequest.StageId;
            mServiceRequest.IsCompleted = stage?.ItemCode == "COMP";
            mServiceRequest.StageName = stage?.ItemName;
            mServiceRequest.CreatedBy = ServiceRequest.CreatedBy;
            mServiceRequest.DelayedReasons = ServiceRequest.DelayedReasons;
            mServiceRequest.IsCritical = ServiceRequest.IsCritical;
            mServiceRequest.IsReportGenerated = context.ServiceReport.Any(x => x.ServiceRequestId == mServiceRequest.Id);
            mServiceRequest.LockRequest = (stage?.ItemCode == "COMP" && mServiceRequest.IsReportGenerated);
            mServiceRequest.AmcServiceQuote = ServiceRequest.AmcServiceQuote;
            mServiceRequest.AmcId = ServiceRequest.AmcId;
            mServiceRequest.BaseAmt = ServiceRequest.BaseAmt;
            mServiceRequest.BaseCurrency = ServiceRequest.BaseCurrency;
            mServiceRequest.CostInUsd = ServiceRequest.CostInUsd;
            mServiceRequest.TotalCostCurrency = ServiceRequest.TotalCostCurrency;
            mServiceRequest.TotalCost = ServiceRequest.TotalCost;
            mServiceRequest.IsNotUnderAmc = ServiceRequest.IsNotUnderAmc;
            mServiceRequest.SiteUserId = ServiceRequest.SiteUserId.HasValue ? ServiceRequest.SiteUserId.Value : null;
            mServiceRequest.SRStages = GetSRStages(ServiceRequest.Id, ServiceRequest.AssignedTo);

            mServiceRequest.EngAction = srEngActionService != null ? srEngActionService.GetSREngActionBySRIdAsync(mServiceRequest.Id).Result : null;
            mServiceRequest.EngComments = srEngCommentsService != null ? srEngCommentsService.GetSREngCommentBySRIdAsync(mServiceRequest.Id).Result : null;
            mServiceRequest.AssignedHistory = srAssignedHistoryService != null ? srAssignedHistoryService.GetSRAssignedHistoryBySRIdAsync(mServiceRequest.Id).Result:null;
            mServiceRequest.ScheduledCalls = engSchedulerService != null ? engSchedulerService.GetEngSchedulerBySRIdAsync(ServiceRequest.Id).Result.Where(x => x.EngId == ServiceRequest.AssignedTo).OrderByDescending(x => x.CreatedOn).ToList() : null;
            return mServiceRequest;
        }

        private ServiceRequestStagesResponse GetSRStages(Guid Id, Guid assignedTo)
        {
            ServiceRequestStagesResponse srStages = new();
            var serRep = context.ServiceReport.Where(x => x.ServiceRequestId == Id).FirstOrDefault();
            srStages.Created = true;
            srStages.Assigned = assignedTo == Guid.Empty ? false : true;
            srStages.MeetingScheduled = context.EngScheduler.Any(x => x.SerReqId == Id);
            srStages.InProgress = serRep != null ? true : false;
            srStages.EngSigned = serRep != null && !string.IsNullOrEmpty(serRep.EngSignature) ? true : false;
            srStages.CustSigned = serRep != null && !string.IsNullOrEmpty(serRep.CustSignature) ? true : false;
            srStages.Completed = serRep != null && serRep.IsCompleted ? true : false;

            return srStages;
        }

        public async Task<SRInstrumentResponse> GetInstrumentDetailByInstrAsync(Guid instrumentId, Guid siteId)
        {
            var ins = await (from i in context.Instrument
                             join ci in context.CustomerInstrument on i.Id equals ci.InstrumentId
                             where ci.InstrumentId == instrumentId && ci.CustSiteId == siteId
                             select new SRInstrumentResponse()
                             {
                                 BrandId = i.BrandId,
                                 BusinessUnitId = i.BusinessUnitId,
                                 InsType = context.ListTypeItems.FirstOrDefault(x => x.Id.ToString() == i.InsType).ItemName,
                                 InsVersion = i.InsVersion,
                                 SerialNos = i.SerialNos,
                                 CustSiteId = ci.CustSiteId,
                                 Id = i.Id,
                                 InstruEngineerId = ci.InstruEngineerId,
                                 OperatorId = ci.OperatorId
                             }).FirstOrDefaultAsync();


            ins.MachineEng = GetSiteContactInfo(ins.InstruEngineerId);
            ins.OperatorEng = GetSiteContactInfo(ins.OperatorId);

            return ins;
        }

        private ContactResponse GetSiteContactInfo(Guid id)
        {
            var contactResponse = new ContactResponse();
            //var contact = context.UserContactMappings.FirstOrDefault(x => x.ContactId == id);
            //if (contact.ContactType == "DR")
            //{
            //    contactResponse =  context.RegionContact.FirstOrDefault(x => x.Id == id).Adapt<ContactResponse>();                
            //}
            //if (contact.ContactType == "CS")
            //{
            contactResponse = context.SiteContact.FirstOrDefault(x => x.Id == id).Adapt<ContactResponse>();
            //}
            return contactResponse;
        }

        public async Task<string> GetServiceRequestNoAsync()
        {
            var srCount = await context.ServiceRequest.ToListAsync();
            return "INC" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + (srCount.Count + 1).ToString();
        }

        public async Task<List<ServiceRequestResponse>> GetServiceRequestByDistAsync(Guid distId)
        {
            var lstServiceRequest = new List<ServiceRequestResponse>();
            
            var serReq = await (from a in context.ServiceRequest.Where(x => !x.IsDeleted) 
                          //join b in context.Instrument  ///.Where(x => x.BusinessUnitId.ToString() == businessUnitId && x.BrandId.ToString() == brandId) 
                          //on a.MachinesNo equals b.Id.ToString()
                          where a.DistId == distId
                          select a).OrderBy(x => x.IsCritical).ThenBy(x => x.SerReqNo).ToListAsync();


            foreach (var r in serReq)
            {
                var x = GetServiceRequestDetail(r);
                if (x != null) lstServiceRequest.Add(x);
            }

            return lstServiceRequest.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<List<ServiceRequestResponse>> GetServiceRequestBySRPIdAsync(Guid serviceReportId)
        {
            var lstServiceRequest = new List<ServiceRequestResponse>();

            var serReq = await (from a in context.ServiceRequest
                                join b in context.ServiceReport on a.Id equals b.ServiceRequestId
                                where b.Id == serviceReportId
                                select a).ToListAsync();

            foreach (var r in serReq)
            {
                var x = GetServiceRequestDetail(r);
                if (x != null) lstServiceRequest.Add(x);
            }

            return lstServiceRequest;
        }

    }
}