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
        IEngSchedulerService engSchedulerService = null, ISRAssignedHistoryService srAssignedHistoryService = null, IConfiguration configuration = null,
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
                                                     join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                                                     join ia in context.InstrumentAllocation.Where(x => businessUnitId.ToString().Contains(x.BusinessUnitId.ToString()) && brandId.ToString().Contains(x.BrandId.ToString()))
                                                     on i.Id equals ia.InstrumentId
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
            serviceRequest.SerReqDate = DateTime.ParseExact(serviceRequest.SerReqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.ToString("dd/MM/yyyy"); // Convert.ToDateTime(serviceRequest.SerReqDate).ToString("dd/MM/yyyy");
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

            var regionId = context.Site.FirstOrDefault(x => x.Id == serviceRequest.SiteId)?.RegionId;
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
            // Get the original service request to check if AssignedTo changed
            var originalServiceRequest = await context.ServiceRequest.AsNoTracking().FirstOrDefaultAsync(x => x.Id == serviceRequest.Id);
            var assignedToChanged = originalServiceRequest != null && originalServiceRequest.AssignedTo != serviceRequest.AssignedTo;
            var newAssignedTo = serviceRequest.AssignedTo;

            serviceRequest.UpdatedOn = DateTime.Now;
            serviceRequest.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(serviceRequest).State = EntityState.Modified;
            await context.SaveChangesAsync();

            // Notify customer when engineer is assigned (AssignedTo changed and is not empty)
            if (assignedToChanged && newAssignedTo != Guid.Empty)
            {
                try
                {
                    var engineer = await context.RegionContact.FirstOrDefaultAsync(x => x.Id == newAssignedTo);
                    if (engineer != null)
                    {
                        await NotifyCustomerForEngineerAssignmentAsync(serviceRequest, engineer);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[UpdateServiceRequestAsync] Error notifying customer: {ex.Message}");
                }
            }

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
            mServiceRequest.CustomerName = context.Customer.FirstOrDefaultAsync(x => x.Id == ServiceRequest.CustId).Result.CustName;
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
            mServiceRequest.AssignedHistory = srAssignedHistoryService != null ? srAssignedHistoryService.GetSRAssignedHistoryBySRIdAsync(mServiceRequest.Id).Result : null;
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

        public async Task NotifyDistributorForServiceRequestAsync(ServiceRequest serviceRequest, string action)
        {
            try
            {
                var currentUserId = Guid.Parse(currentUserService.GetUserId());
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (userProfile == null)
                    return;

                // Only notify if user is Distributor
                if (userProfile.SegmentCode == "RCUST")
                {
                    var distributor = await context.Distributor.FirstOrDefaultAsync(d => d.Id == serviceRequest.DistId);
                    if (distributor == null)
                        return;

                    var customer = await context.Customer.FirstOrDefaultAsync(c => c.Id == serviceRequest.CustId);
                    var site = await context.Site.FirstOrDefaultAsync(s => s.Id == serviceRequest.SiteId);

                    var distributorUsers = await context.VW_UserProfile.Where(x => x.SegmentCode == "RDTSP" && x.EntityParentId == serviceRequest.DistId).ToListAsync();

                    var statusAction = action == "created" ? "SUBMITTED" : "UPDATED";
                    var notificationMessage = $"Service Request {serviceRequest.SerReqNo} has been {statusAction}";
                    if (customer != null)
                        notificationMessage += $" for customer '{customer.CustName}'";
                    if (site != null)
                        notificationMessage += $" at site '{site.CustRegName}'";

                    foreach (VW_UserProfile user in distributorUsers)
                    {
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = notificationMessage,
                            RoleId = userProfile.RoleId,
                            RaisedBy = currentUserService.GetUserId(),
                            UserId = user.UserId,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = currentUserId,
                            CreatedOn = DateTime.Now,
                            UpdatedBy = currentUserId,
                            UpdatedOn = DateTime.Now
                        };

                        await context.Notifications.AddAsync(notification);
                        await context.SaveChangesAsync();

                        // Send email to distributor
                        var emailBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h3>Service Request Notification</h3>
                        <p><strong>Service Request Number:</strong> {serviceRequest.SerReqNo}</p>
                        <p><strong>Status:</strong> {statusAction}</p>
                        <p><strong>Customer:</strong> {customer?.CustName ?? "N/A"}</p>
                        <p><strong>Site:</strong> {site?.CustRegName ?? "N/A"}</p>
                        <p><strong>Request Date:</strong> {serviceRequest.SerReqDate}</p>
                        <p><strong>Message:</strong> {notificationMessage}</p>
                        <p><strong>Action Required:</strong> Please review this service request in the CIM dashboard.</p>
                        <hr/>
                        <p><em>This is an automated notification. Please do not reply to this email.</em></p>
                    </body>
                    </html>";

                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            var commonMethods = new CommonMethods(context, currentUserService, configuration);
                            commonMethods.SendEmailMethod(
                                user.Email,
                                emailBody,
                                $"Service Request {serviceRequest.SerReqNo} - {statusAction}"
                            );
                        }
                    }
                }
                else if (userProfile.SegmentCode == "RENG" && action == "updated")
                {
                    await NotifyDistributorForEngineerResponseAsync(serviceRequest, userProfile);
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForServiceRequest] Error: {ex.Message}");
            }
        }

        public async Task NotifyDistributorForEngineerResponseAsync(ServiceRequest serviceRequest, VW_UserProfile userProfile)
        {
            try
            {
                var currentUserId = Guid.Parse(currentUserService.GetUserId());                               

                var distributor = await context.Distributor.FirstOrDefaultAsync(d => d.Id == serviceRequest.DistId);
                if (distributor == null)
                    return;
                             
                var engineer = await context.RegionContact.FirstOrDefaultAsync(rc => rc.Id == userProfile.ContactId);

                // Get all distributor users with RDTSP segment code
                var distributorUsers = await context.VW_UserProfile.Where(x => x.SegmentCode == "RDTSP" && x.EntityParentId == serviceRequest.DistId).ToListAsync();

                // Get action name from VW_ListItems
                var status = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == serviceRequest.StatusId)?.ItemName;

                var notificationMessage = $"Engineer {engineer?.FirstName} {engineer?.LastName} has {status.ToLower()} the Service Request {serviceRequest.SerReqNo}";

                foreach (VW_UserProfile distributorUser in distributorUsers)
                {
                    var notification = new Notifications
                    {
                        Id = Guid.NewGuid(),
                        Remarks = notificationMessage,
                        RoleId = distributorUser.RoleId,
                        RaisedBy = currentUserService.GetUserId(),
                        UserId = distributorUser.UserId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = currentUserId,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = currentUserId,
                        UpdatedOn = DateTime.Now
                    };

                    await context.Notifications.AddAsync(notification);
                    await context.SaveChangesAsync();

                    // Send email to distributor
                    var emailBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h3>Engineer Response to Service Request</h3>
                        <p><strong>Service Request Number:</strong> {serviceRequest.SerReqNo}</p>
                        <p><strong>Engineer Name:</strong> {engineer?.FirstName} {engineer?.LastName}</p>
                        <p><strong>Action Taken:</strong> <strong style='color: {(status.ToUpper() == "ACCEPTED" ? "green" : "red")};'>{status}</strong></p>                        
                        <p><strong>Request Date:</strong> {serviceRequest.SerReqDate}</p>                        
                        <p><strong>Action Required:</strong> Please review the engineer's response in the CIM dashboard.</p>
                        <hr/>
                        <p><em>This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator</em></p>
                    </body>
                    </html>";
                    //<p><strong>Customer:</strong> {customer?.CustName ?? "N/A"}</p>
                    //<p><strong>Site:</strong> {site?.CustRegName ?? "N/A"}</p>
                    // { (!string.IsNullOrEmpty(comments) ? $"<p><strong>Comments:</strong> {comments}</p>" : "")}
                    if (!string.IsNullOrEmpty(distributorUser.Email))
                    {
                        var commonMethods = new CommonMethods(context, currentUserService, configuration);
                        commonMethods.SendEmailMethod(
                            distributorUser.Email,
                            emailBody,
                            $"Service Request {serviceRequest.SerReqNo} - Engineer Response: {status}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyDistributorForEngineerResponse] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Notifies customer site contact when engineer is assigned to service request
        /// </summary>
        public async Task NotifyCustomerForEngineerAssignmentAsync(Domain.Entities.ServiceRequest serviceRequest, RegionContact engineer)
        {
            try
            {
                if (engineer == null || serviceRequest == null) return;

                var site = await context.Site.FirstOrDefaultAsync(x => x.Id == serviceRequest.SiteId);
                if (site == null) return;

                var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == site.CustomerId);
                var siteContacts = await context.VW_UserProfile.Where(x => (x.SegmentCode == "RCUST" || x.SegmentCode == "RENG")
                && x.EntityChildId == serviceRequest.SiteId && x.EntityParentId == serviceRequest.CustId).ToListAsync();

                if (siteContacts.Count == 0) return;

                foreach (var contact in siteContacts)
                {
                    try
                    {
                        // Create in-app notification
                        var notification = new Notifications
                        {
                            Id = Guid.NewGuid(),
                            Remarks = $"Engineer {engineer.FirstName} {engineer.LastName} has been assigned to Service Request {serviceRequest.SerReqNo}",
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
                        await SendCustomerAssignmentEmailAsync(contact, engineer, serviceRequest, site, customer);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForEngineerAssignment] Error notifying contact {contact.ContactId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotifyCustomerForEngineerAssignment] Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends email notification to customer when engineer is assigned
        /// </summary>
        private async Task SendCustomerAssignmentEmailAsync(
            VW_UserProfile contact,
            RegionContact engineer,
            Domain.Entities.ServiceRequest serviceRequest,
            Site site,
            Customer customer)
        {
            try
            {
                if (string.IsNullOrEmpty(contact.Email)) return;

                var emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h3>👨‍🔧 Engineer Assigned to Your Service Request</h3>
                    
                    <div style='background-color: #d1ecf1; padding: 15px; border-left: 4px solid #0c5460; margin: 10px 0;'>
                        <p><strong>✓ INFO:</strong> An engineer has been assigned to address your service request.</p>
                    </div>
                    
                    <h4>Engineer Assignment Details:</h4>
                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Request #:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqNo}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Assigned Engineer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong style='color: #0c5460;'>{engineer.FirstName} {engineer.LastName}</strong></td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Customer:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customer?.CustName ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Site Location:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{site?.CustRegName ?? "N/A"}</td>
                        </tr>
                        <tr style='background-color: #f8f9fa;'>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Request Type:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == serviceRequest.VisitType)?.ItemName ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>Request Date:</strong></td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{serviceRequest.SerReqDate}</td>
                        </tr>
                    </table>
                    
                    <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #2196F3;'>
                        <p><strong>What's Next:</strong></p>
                        <ul>
                            <li>The assigned engineer will contact you shortly with schedule details</li>
                            <li>Please ensure someone is available at the site during the scheduled visit</li>
                            <li>Have any relevant documentation ready for the engineer</li>
                            <li>You will receive a visit confirmation email with date and time</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 20px 0; border: none; border-top: 2px solid #ddd;' />
                    
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is an automated notification generated when engineers are assigned to service requests.</em>
                    </p>
                    <p style='color: #666; font-size: 12px;'>
                        <em>This is a system-generated email. Please contact your administrator for support.</em>
                    </p>
                </body>
                </html>";

                var subject = $"Engineer Assignment - Service Request {serviceRequest.SerReqNo}";

                var cm = new CommonMethods(context, currentUserService, configuration);
                cm.SendEmailMethod(contact.Email, emailBody, subject);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SendCustomerAssignmentEmailAsync] Error sending email: {ex.Message}");
            }
        }

    }
}