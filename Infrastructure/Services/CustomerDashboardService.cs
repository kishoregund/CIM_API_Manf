using Application.Features.AMCS.Responses;
using Application.Features.AppBasic.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;
using Application.Features.Identity.Users;
using Application.Features.ServiceRequests.Responses;
using Application.Features.Spares.Responses;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace Infrastructure.Services
{
    public class CustomerDashboardService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : ICustomerDashboardService
    {
        public async Task<InstrumentOwnershipResponse> GetCostOfOwnerShipAsync(string id)
        {
            //var result = new Message();
            var insOwner = new InstrumentOwnershipResponse();
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                //var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;

                //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

                //var instrumentDL = new InstrumentDL(context);
                //var privilages = context.Vw_Privilages.Where(x => x.UserId == userId).ToList();
                //var curPrivilage = privilages.FirstOrDefault(x => x.ScreenCode == "SCOUN" && x.UserName != "admin");
                //var offerRequestPrevilages = privilages.FirstOrDefault(x => x.ScreenCode == "OFREQ" && x.UserName != "admin");

                var lstCurrencies = await context.Currency.Where(x => !x.IsDeleted).ToListAsync();
                var offerRequest = context.OfferRequest.Where(x => !x.IsDeleted).ToList();

                //if (offerRequestPrevilages.UserName != "admin" && offerRequestPrevilages.PrivilageCode != "PARTS" && (offerRequestPrevilages._create || offerRequestPrevilages._read || offerRequestPrevilages._update || offerRequestPrevilages._delete))
                //{
                //    offerRequest = offerRequest.Where(x => x.Createdby == userId);
                //}

                //if (curPrivilage != null && curPrivilage.PrivilageCode != "PARTS" && (curPrivilage._create || curPrivilage._read || curPrivilage._update || curPrivilage._delete))
                //{
                //    currency = currency.Where(x => x.Createdby == userId);
                //}

                CustomerInstrumentService customerInstrumentService = new(context, currentUserService, configuration);

                var instrument = customerInstrumentService.GetAssignedCustomerInstrumentsAsync(string.Empty, string.Empty).Result
                    .FirstOrDefault(x=>x.InstrumentId.ToString() == id);

                if (instrument != null)
                {

                    //var insType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == instrument.InsType)?.Itemname;
                    insOwner = new InstrumentOwnershipResponse
                    {
                        InsCost = instrument.Cost * instrument.BaseCurrencyAmt ?? 0,
                        InsCostCurrency = lstCurrencies.FirstOrDefault(x => x.Id == instrument.CurrencyId)?.Symbol,
                        DateOfPurchase = instrument.DateOfPurchase,
                        InsSerialNo = instrument.InsTypeName + "-" + instrument.SerialNos,
                        InstrumentId = instrument.Id.ToString(),
                        OwnerShip = new List<CostOfOwnership>(),
                    };


                    var insData = (from ci in context.CustomerInstrument
                                   join ai in context.AMCInstrument on ci.InstrumentId equals ai.InstrumentId
                                   join a in context.AMC on ai.AMCId equals a.Id
                                   join li in context.VW_ListItems on a.ServiceType equals li.ListTypeItemId.ToString()
                                   where ci.InstrumentId == instrument.Id
                                   select new
                                   {
                                       ci.Cost,
                                       ci.InstrumentId,
                                       ci.DateOfPurchase,
                                       a.CurrencyId,
                                       a.Zerorate,
                                       a.SDate,
                                       a.ServiceType,
                                       li.ItemName,
                                       li.ItemCode,
                                       a.BaseCurrencyAmt
                                   }).OrderBy(x => x.SDate).ToList();

                    //var insData = context.Vw_DashInsOwnershipCost.Where(x => x.InstrumentId == instrument.Id).OrderBy(x => x.SDate).ToList();




                    var offerAmt = (from op in context.OfferRequestProcess
                                    join oreq in offerRequest on op.OfferRequestId equals oreq.Id
                                    select new { op.PayAmtCurrencyId, op.PayAmt, op.BaseCurrencyAmt, oreq.Instruments })
                        .Where(x => x.Instruments.Contains(instrument.Id.ToString())).ToList();

                    var year = 0;

                    foreach (var item in insData)
                    {
                        var data = new CostOfOwnership
                        {
                            ServiceContractYrs = year + 1,
                            AverageCostOfOwnershipAnnualy = 0,
                            BreakdownVisitCost = 0,
                            CostOfOwnershipAnnualy = 0,
                            ServiceContractCost = 0,
                            SparePartsCost = 0,
                            TotalAnnualCost = 0,
                            TotalCummulativeCost = 0
                        };

                        if (item.ItemCode == "AMC")
                        {
                            if (decimal.IsPositive((decimal)item.Zerorate) && item.BaseCurrencyAmt.HasValue && decimal.IsPositive((decimal)item.BaseCurrencyAmt.Value))
                            {
                                data.ServiceContractCost += (decimal)(item.Zerorate * item.BaseCurrencyAmt);
                            }
                            else data.ServiceContractCost = 0;

                            data.ServiceContractCostCurrencyId =
                                lstCurrencies.FirstOrDefault(x => x.Id == item.CurrencyId)?.Symbol;
                        }

                        else if (item.ItemCode == "BRKDW")
                        {
                            if (decimal.IsPositive((decimal)item.Zerorate) && item.BaseCurrencyAmt.HasValue && decimal.IsPositive((decimal)item.BaseCurrencyAmt.Value))
                            {
                                data.BreakdownVisitCost += (decimal)(item.Zerorate * item.BaseCurrencyAmt);
                            }
                            else data.BreakdownVisitCost = 0;
                            data.ServiceContractCostCurrencyId = lstCurrencies.FirstOrDefault(x => x.Id == item.CurrencyId)?.Symbol;
                        }
                        insOwner.OwnerShip.Add(data);
                        year++;
                    }

                    year = 0;

                    foreach (var item in offerAmt.Where(item => insOwner.OwnerShip.Count > year))
                    {
                        insOwner.OwnerShip.ElementAt(year).SparePartsCost += item.PayAmt * item.BaseCurrencyAmt;
                        insOwner.OwnerShip.ElementAt(year).SparePartsCostCurrencyId = lstCurrencies
                            .FirstOrDefault(x => x.Id == item.PayAmtCurrencyId)?.Symbol;
                        year++;
                    }

                    decimal totalCumulativeCost = 0;

                    foreach (var item in insOwner.OwnerShip)
                    {
                        item.TotalAnnualCost = item.SparePartsCost + item.BreakdownVisitCost + item.ServiceContractCost;
                        totalCumulativeCost += item.TotalAnnualCost;
                        item.TotalCummulativeCost = totalCumulativeCost;

                        if (decimal.IsPositive(item.TotalCummulativeCost) && decimal.IsPositive(insOwner.InsCost))
                            item.CostOfOwnershipAnnualy = (item.TotalCummulativeCost / insOwner.InsCost) * 100;

                        if (decimal.IsPositive(item.CostOfOwnershipAnnualy))
                            item.AverageCostOfOwnershipAnnualy = (item.CostOfOwnershipAnnualy / item.ServiceContractYrs) * 100;
                    }

                    //result.Result = true;
                    //result.Object = insOwner;
                   
                }
                
                
            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // Variable is declared but never used
            return insOwner;
        }

        public async Task<object> GetCostDataAsync(DashboardDateRequest dashboardDateModel)
        {
            //var result = new Message();

            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            var offerRequestService = new OfferRequestService(context, currentUserService, configuration);
            var commonMethods = new CommonMethods(context, currentUserService, configuration);

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

            var lstOfferReq = offerRequestService.GetOfferRequestsAsync().Result;

            //var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var offerRequestProcess = context.OfferRequestProcess.Where(x => !x.IsDeleted);

            //if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            //{
            //    offerRequestProcess = offerRequestProcess.Where(x => x.CreatedBy == userId);
            //}

            var payRecStage = context.VW_ListItems.FirstOrDefault(y => y.ItemCode == "PYRCT" && y.ListCode == "OFRQP");

            decimal poCost = 0;

            foreach (var item in lstOfferReq)
            {
                dashboardDateModel.CreatedOn = item.CreatedOn;
                if (commonMethods.GetDateDiff(dashboardDateModel)) poCost += item.TotalAmt * item.BasePCurrencyAmt;
            }

            var aMCDL = new AmcService(context, currentUserService);
            var lstAmc = await aMCDL.GetAllAsync();
            var amcSTId = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "SERTY" && x.ItemCode == "AMC")?.ListTypeItemId;

            decimal? othrCost = 0;
            decimal? amcCost = 0;

            foreach (var item in lstAmc)
            {
                dashboardDateModel.CreatedOn = item.CreatedOn;
                if (commonMethods.GetDateDiff(dashboardDateModel))
                {
                        amcCost += item.Zerorate * item.BaseCurrencyAmt;
                    //else othrCost += item.Zerorate * item.BaseCurrencyAmt;
                }
            }

            var lstSites = await commonMethods.GetSitesByUserIdAsync();
            var serviceRequests = (from sr in context.ServiceRequest.Where(x => lstSites.Contains(x.SiteId.ToString())).ToList()
                                   join i in context.Instrument on sr.MachinesNo equals i.Id.ToString()
                                   select sr).ToList().Adapt<List<ServiceRequestResponse>>();



            foreach (ServiceRequestResponse sr in serviceRequests)
            {
                var visitType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Guid.Parse(sr.VisitType)).ItemCode;
                switch (visitType)
                {
                    case "PREV":
                        othrCost += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "BRKDW":
                        othrCost += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "ONCAL":
                        othrCost += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "PLAN":
                        othrCost += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;
                }
            }


            return new { othrCost, amcCost, poCost };
        }

        public async Task<List<ServiceRequestResponse>> GetAllServiceRequestAsync()
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService(context, currentUserService);
            var lstServiceRequest = await serviceRequestService.GetDetailServiceRequestsAsync(string.Empty, string.Empty);
            /*
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

            var serviceRequest = new List<ServiceRequest>();
            var commonMethods = new CommonMethods(context, currentUserService);

            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");

            //var bUs = commonMethods.GetBusinessUnitList(userId, bUId);

            var serReq = (from a in context.ServiceRequest.Where(x => !x.IsDeleted)
                          join b in context.Instrument//.Where(x => bUs.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId)) // for customer its all BU and brands
                              on a.MachinesNo equals b.Id.ToString()
                          select a);
            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    serReq = serReq.Where(x => x.Createdby == userId);

            //var user = context.Users.FirstOrDefault(x => x.Id == userId);

            var lstSites = commonMethods.GetSitesByUserIdAsync();
            //var contactMapped = context.Contactmapping.FirstOrDefault(x => x.Contactid == user.Contactid);
            //var userType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == contactMapped.Mappedfor)
            //    ?.Itemname;

            //var roleId = context.VW_UserProfiles.FirstOrDefault(x => x.Userid == userId);
            //var role = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == roleId.Roleid)?.ItemCode;

            //if (role?.ToLower() != "rcust") return new Message { Result = false, ResultMessage = "User should be Customer" };

            if (userProfile.ContactType.ToLower() == "cs")
            {
                serviceRequest = serReq.Where(x => x.SiteId == userProfile.EntityChildId)
                    .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.CreatedOn).ToList();
            }
            //else if (userType?.ToLower() == "customer")
            //{
            //    serviceRequest = serReq
            //        .Where(x => x.Custid == contactMapped.Parentid && lstSites.Contains(x.Siteid))
            //        .OrderByDescending(x => x.IsCritical).ThenByDescending(x => x.Createdon).ToList();
            //}

            serviceRequest = serviceRequest.Where(x => lstSites.Contains(x.SiteId.ToString())).ToList();


            var lstServiceRequest = serviceRequest.Select(r => GetServiceRequest(r))
                .Where(x => x != null).ToList();

            var result = new Message
            {
                Result = true,
                Object = lstServiceRequest,
            };

            return result;
            */

            return lstServiceRequest;
        }

        /* 
        private ServiceRequestModel GetServiceRequest(ServiceRequest ServiceRequest, string bUId, string brandId, string userId)
        {
            var commonMethods = new CommonMethods(context);
            if (ServiceRequest == null) return null;


            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var brands = commonMethods.GetBrandList(userId, brandId);
            var bUs = commonMethods.GetBusinessUnitList(userId, bUId);
            var serReq = (from s in context.ServiceRequest.Where(x => !x.Isdeleted) join b in context.Instrument.Where(x => bUs.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId)) on s.Machinesno equals b.Id select s);
            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
                serReq = serReq.Where(x => x.Createdby == userId);

            var service = serReq.FirstOrDefault(x => x.Id == ServiceRequest.Id);
            if (service == null) return null;
            var stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.Stageid);
            var mServiceRequest = new ServiceRequestModel();
            mServiceRequest.Id = ServiceRequest.Id;
            mServiceRequest.Createdon = ServiceRequest.Createdon;
            mServiceRequest.Serreqno = ServiceRequest.Serreqno;
            mServiceRequest.Accepted = false;
            mServiceRequest.Escalation = ServiceRequest.Escalation;
            mServiceRequest.IsReportGenerated = ServiceRequest.IsReportGenerated;
            mServiceRequest.AcceptedDate = ServiceRequest.AcceptedDate;
            mServiceRequest.Alarmdetails = ServiceRequest.Alarmdetails;
            mServiceRequest.Assignedto = ServiceRequest.Assignedto;
            if (ServiceRequest.Assignedto != null && ServiceRequest.Assignedto != "")
            {
                var assignTo = context.Contact.FirstOrDefault(x => x.Id == ServiceRequest.Assignedto);
                mServiceRequest.AssignedtoName = assignTo != null ? assignTo.Fname + ' ' + assignTo.Lname : "";
            }
            mServiceRequest.Breakdowntype = ServiceRequest.Breakdowntype;
            mServiceRequest.Breakoccurdetailsid = ServiceRequest.Breakoccurdetailsid;
            mServiceRequest.Breakoccurdetails = context.VW_ListItems.Where(x => x.ListTypeItemId == ServiceRequest.Breakoccurdetailsid)?.FirstOrDefault()?.Itemname;
            mServiceRequest.Companyname = ServiceRequest.Companyname;
            mServiceRequest.Complaintregisname = ServiceRequest.Complaintregisname;
            mServiceRequest.Contactperson = ServiceRequest.Contactperson;
            mServiceRequest.Country = ServiceRequest.Country;
            mServiceRequest.CountryName = context.Country.FirstOrDefault(x => x.Id == ServiceRequest.Country)?.Name;
            mServiceRequest.Currentinstrustatus = ServiceRequest.Currentinstrustatus;
            mServiceRequest.CurrentinstrustatusName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.Currentinstrustatus)?.Itemname;
            mServiceRequest.Distributor = context.Distributor.Where(x => x.Id == ServiceRequest.Distid)?.FirstOrDefault()?.Distname;
            mServiceRequest.Email = ServiceRequest.Email;
            mServiceRequest.Isactive = ServiceRequest.Isactive;
            mServiceRequest.Isrecurring = ServiceRequest.Isrecurring;
            mServiceRequest.Machinesno = ServiceRequest.Machinesno;
            mServiceRequest.Machmodelname = ServiceRequest.Machmodelname;
            mServiceRequest.Machmodelnametext = context.VW_ListItems.Where(x => x.ListTypeItemId == ServiceRequest.Machmodelname)?.FirstOrDefault()?.Itemname;
            mServiceRequest.Machengineer = ServiceRequest.Machengineer;
            mServiceRequest.Operatoremail = ServiceRequest.Operatoremail;
            mServiceRequest.Operatorname = ServiceRequest.Operatorname;
            mServiceRequest.Operatornumber = ServiceRequest.Operatornumber;
            mServiceRequest.Recurringcomments = ServiceRequest.Recurringcomments;
            mServiceRequest.Registrarphone = ServiceRequest.Registrarphone;
            mServiceRequest.Requesttime = ServiceRequest.Requesttime;
            mServiceRequest.Resolveaction = ServiceRequest.Resolveaction;
            mServiceRequest.Samplehandlingtype = ServiceRequest.Samplehandlingtype;
            mServiceRequest.Serreqdate = ServiceRequest.Serreqdate;
            mServiceRequest.Sitename = context.Site.FirstOrDefault(x => x.Id == ServiceRequest.Siteid)?.Custregname;
            mServiceRequest.Visittype = ServiceRequest.Visittype;
            mServiceRequest.VisittypeName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.Visittype)?.Itemname;
            mServiceRequest.Xraygenerator = ServiceRequest.Xraygenerator;
            mServiceRequest.Siteid = ServiceRequest.Siteid;
            mServiceRequest.Custid = ServiceRequest.Custid;
            mServiceRequest.Distid = ServiceRequest.Distid;
            mServiceRequest.Requesttypeid = ServiceRequest.Requesttypeid;
            mServiceRequest.Requesttype = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.Requesttypeid)?.Itemname;
            mServiceRequest.Subrequesttypeid = ServiceRequest.Subrequesttypeid;
            mServiceRequest.Statusid = ServiceRequest.Statusid;
            mServiceRequest.StatusName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == ServiceRequest.Statusid)?.Itemname;
            mServiceRequest.Remarks = ServiceRequest.Remarks;
            mServiceRequest.Serresolutiondate = ServiceRequest.Serresolutiondate;
            mServiceRequest.Sdate = ServiceRequest.Sdate;
            mServiceRequest.Edate = ServiceRequest.Edate;
            mServiceRequest.MachineModelName = context.Instrument.Where(x => x.Id == ServiceRequest.Machinesno)?.FirstOrDefault()?.Serialnos;
            mServiceRequest.Stageid = ServiceRequest.Stageid;
            mServiceRequest.StageName = stage?.Itemname;
            mServiceRequest.IsCompleted = stage?.ItemCode == "COMP";
            mServiceRequest.Createdby = ServiceRequest.Createdby;
            mServiceRequest.DelayedReasons = ServiceRequest.DelayedReasons;
            mServiceRequest.IsCritical = ServiceRequest.IsCritical;
            mServiceRequest.IsReportGenerated =
                context.ServiceReport.Any(x => x.Servicerequestid == mServiceRequest.Id);
            mServiceRequest.LockRequest = (stage?.ItemCode == "COMP" && mServiceRequest.IsReportGenerated);

            var c = new SREngCommentDL(context);
            var a = new SREngineerActionDL(context);
            var h = new SRAssignedHistoriesDL(context);
            var engScheduler = new EngSchedulerDL(context);

            mServiceRequest.EngAction = a.GetEngineerActionsBySRId(mServiceRequest.Id);
            mServiceRequest.EngComments = c.GetEngCommentsBySRId(mServiceRequest.Id);
            mServiceRequest.AssignedHistory = h.GetAssignedHistoryBySRId(mServiceRequest.Id);
            mServiceRequest.ScheduledCalls = engScheduler.GetEngSchedulerByEngId(ServiceRequest.Assignedto, userId, bUId, brandId).Where(x => x.SerReqId == ServiceRequest.Id).OrderByDescending(x => x.Createdon).ToList();
            return mServiceRequest;
        }

        */

        public async Task<List<AmcResponse>> GetAllAmcAsync()
        {

            AmcService amcService = new AmcService(context, currentUserService);
            return await amcService.GetAllAsync();
            /*
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

            var lstAmc = new List<AMCModel>();

            var commonMethods = new CommonMethods(context);
            var regionsprofile = commonMethods.GetUserDistRegionsByUserId(userId);
            var lstRegionsprofile = regionsprofile?.Split(',');
            var bUs = commonMethods.GetBusinessUnitList(userId, bUId);
            var brands = commonMethods.GetBrandList(userId, brandId);

            var sites = commonMethods.GetUserSitesByUserId(userId);
            var lstSites = sites.Split(",");
            var user = context.Users.FirstOrDefault(x => x.Id == userId);

            var amc = (from a in context.AMC.Where(x => !x.Isdeleted)
                       join b in context.AmcInstrument.Where(x => !x.Isdeleted) on a.Id equals b.AmcId
                       join c in context.Instrument.Where(x => bUs.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId))
                           on b.InstrumentId equals c.Id
                       select a);

            var contactMapped = context.Contactmapping.FirstOrDefault(x => x.Contactid == user.Contactid);
            var userType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == contactMapped.Mappedfor)
                ?.Itemname;
            var privilage = context.Vw_Privilages.FirstOrDefault(x =>
                x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");

            if (privilage != null && privilage.PrivilageCode != "PARTS" &&
                (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                amc = amc.Where(x => x.Createdby == userId);
            }

            if (userType.ToLower() == "site")
            {
                var Amc = amc.Where(x => x.CustSite == contactMapped.Parentid).ToList();

                lstAmc.AddRange(Amc.Select(a => GetAMC(a.Id, userId)).Where(AMC => AMC != null));
            }
            else if (userType.ToLower() == "customer")
            {
                var AMc = (from aMc in amc.Where(x => x.Isdeleted == false)
                           join cust in context.Customer.Where(x => x.Id == contactMapped.Parentid) on aMc.Billto equals
                               cust.Id
                           select (new { aMc, cust }))
                    .Where(x => lstRegionsprofile.Contains(x.cust.Defdistregionid) && lstSites.Contains(x.aMc.CustSite))
                    .ToList();

                lstAmc.AddRange(AMc.Select(a => GetAMC(a.aMc.Id, userId)).Where(AMC => AMC != null));
            }


            //lstAmc = lstAmc.DistinctBy<AMCModel>.(x => x.Id).ToList();

            var result = new Message
            {
                Result = true,
                Object = lstAmc
            };

            */

        }

        /*
        private AMCModel GetAMC(string id, string userId)
        {
            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var amc = context.AMC.Where(x => !x.Isdeleted);

            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                amc = amc.Where(x => x.Createdby == userId);
            }

            var Amc = amc.Where(x => x.Id == id).OrderBy(x => x.Project).FirstOrDefault();
            if (Amc != null)
            {
                var isCompleted = false;
                var stage = "";
                var stageLst = context.AMCStages.Where(x => x.AMCId == Amc.Id && x.IsCompleted == true)?.OrderByDescending(x => x.UpdatedOn).ToList();

                if (stageLst.Count > 0)
                {
                    stage = stageLst[0]?.Stage;
                    stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == stage)?.Itemname;
                    stage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stage);

                    var compStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "COMPT" && x.ListCode == "AMCSG");
                    var completedStage = stageLst.FirstOrDefault(x => x.Stage == compStage.ListTypeItemId);
                    isCompleted = completedStage != null;

                }

                var mAMC = new AMCModel
                {
                    Id = Amc.Id,
                    Isactive = Amc.Isactive,
                    Createdon = Amc.Createdon,
                    CustSite = Amc.CustSite,
                    CustSiteName = context.Site.FirstOrDefault(x => x.Id == Amc.CustSite)?.Custregname,
                    Billtoid = Amc.Billto,
                    Billto = context.Customer.FirstOrDefault(x => x.Id == Amc.Billto)?.Custname,
                    Servicequote = Amc.Servicequote,
                    Sqdate = Amc.Sqdate,
                    Edate = Amc.Edate,
                    Sdate = Amc.Sdate,
                    Project = Amc.Project,
                    Servicetype = Amc.Servicetype,
                    Brand = Amc.Brand,
                    Currency = Amc.Currency,
                    Zerorate = Amc.Zerorate,
                    BaseCurrencyAmt = Amc.BaseCurrencyAmt,
                    Tnc = Amc.Tnc,
                    StageName = stage,
                    PaymentTerms = Amc.PaymentTerms,
                    SecondVisitDate = Amc.SecondVisitDate,
                    FirstVisitDate = Amc.FirstVisitDate,
                    IsCompleted = isCompleted,
                    AMCServiceType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Amc.Servicetype)?.Itemname,
                    AMCServiceTypeCode = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Amc.Servicetype)?.ItemCode
                };

                return mAMC;
            }
            return null;
        }

        */

        public async Task<List<VW_SparesRecommended>> GetSparePartsRecommendedAsync()
        {
            var commonMethods = new CommonMethods(context, currentUserService, configuration);

            var sites = await commonMethods.GetSitesByUserIdAsync();

            return context.VW_SparesRecommended.Where(x => sites.Contains(x.SiteId.ToString())).OrderBy(x => x.QtyRecommended).ToList();

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var userprofile = context.VW_UserProfiles.FirstOrDefault(x => x.Userid == userId);
            //var contactId = userprofile?.Contactid;
            /*
            var lstSparePartsRecommended = new List<VW_SparesRecommended>();

            
            var parentId = context.Contactmapping.FirstOrDefault(x => x.Contactid == contactId);
            var user = context.Users.FirstOrDefault(x => x.Contactid == contactId);
            var brands = commonMethods.GetBrandList(user.Id, brandId);
            var bUs = commonMethods.GetBusinessUnitList(user.Id, bUId);
            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == user.Id && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var spr = context.VW_SparesRecommended.Where(x => !x.Isdeleted && brands.Contains(x.BrandId) && bUs.Contains(x.BusinessUnitId));
            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
                spr = spr.Where(x => x.Createdby == user.Id);

            var regionsprofile = commonMethods.GetUserDistRegionsByUserId(user.Id);
            var lstRegionsprofile = regionsprofile?.Split(',');

            var contactMapped = context.Contactmapping.FirstOrDefault(x => x.Contactid == contactId);
            var userType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == contactMapped.Mappedfor)?.Itemname;
            var srereq = new List<ServiceRequest>();

            if (userType.ToLower() == "site")
                srereq = context.ServiceRequest.Where(x => x.Siteid == parentId.Parentid).ToList();

            else if (userType.ToLower() == "customer")
                srereq = context.ServiceRequest.Where(x
                    => x.Custid == parentId.Parentid).ToList();

            foreach (var item in srereq)
            {
                lstSparePartsRecommended.AddRange(spr
                    .Where(x => x.ServiceRequestId == item.Id && lstRegionsprofile.Contains(x.SiteRegion))
                    .OrderBy(x => x.QtyRecommended).ToList());
            }

            var result = new Message
            {
                Result = true,
                Object = lstSparePartsRecommended
            };

            return result;
            */
        }

        public async Task<CustomerResponse> GetCustomerDetailsAsync()
        {

            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            CustomerResponse customer = (await context.Customer.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == userProfile.EntityParentId)).Adapt<CustomerResponse>();
            var sites = userProfile.CustSites.Split(',');
            customer.Sites = (await context.Site.Where(x => x.CustomerId == customer.Id && sites.Contains(x.Id.ToString()) ).ToListAsync()).Adapt<List<SiteResponse>>();
            foreach (SiteResponse site in customer.Sites)
            {
                site.SiteContacts = await context.SiteContact.Where(x => x.SiteId == site.Id).ToListAsync();
            }

            return customer;

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            //var rFor = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "CUST").ListTypeItemId;
            //var up = context.VW_UserProfiles.FirstOrDefault(x => x.Userid == userId);

            //var resultObj = GetCustomer(customer.Id, rFor, userId);
            //var result = new Message
            //{
            //    Result = true,
            //    Object = resultObj
            //};

            //return result;
        }
        
        /*
        private CustomerModel GetCustomer(string id, string rFor, string userId)
        {
            var conObj = new ContactDL(context);
            var Customer = new Customer();
            if (rFor == null || rFor == "")
                rFor = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "CUST").ListTypeItemId;
            var customer = context.Customer.Where(x => !x.Isdeleted);

            var up = context.VW_UserProfiles.FirstOrDefault(x => x.Userid == userId);
            Customer = customer.FirstOrDefault(x => x.Id == up.EntityParentId);

            if (Customer == null) return null;

            var address = context.Address.FirstOrDefault(x => x.Parentid == id && x.Addressfor == rFor);
            var contacts = conObj.GetParentsContacts(id, rFor);
            var custSites = GetCustomerSites(id, userId);

            var mCustomer = new CustomerModel
            {
                Id = Customer.Id,
                IndustrySegment = Customer.IndustrySegment,
                Custname = Customer.Custname,
                Defdistid = Customer.Defdistid,
                Defdistregionid = Customer.Defdistregionid,
                DefDistRegion = context.DistRegions.FirstOrDefault(x => x.Id == Customer.Defdistregionid)?.Region,
                Defdist = context.Distributor.FirstOrDefault(x => x.Id == Customer.Defdistid).Distname,
                Isactive = Customer.Isactive,
                Countryid = Customer.Countryid,
                Code = Customer.Code,
                CountryName = context.Country.FirstOrDefault(x => x.Id == Customer.Countryid)?.Name,

                Address = new AddressModel
                {
                    Area = address.Area,
                    City = address.City,
                    Countryid = address.Countryid,
                    CountryName = context.Country.FirstOrDefault(x => x.Id == address.Countryid) != null
                        ? context.Country.FirstOrDefault(x => x.Id == address.Countryid).Name
                        : "",
                    Geolat = address.Geolat,
                    Geolong = address.Geolong,
                    Place = address.Place,
                    Street = address.Street,
                    Zip = address.Zip,
                    IsActive = address.Isactive
                },

                Contacts = contacts,
                Sites = custSites
            };

            return mCustomer;
        }
        private SiteModel GetSite(string id, string rFor)
        {
            var conObj = new ContactDL(context);
            if (string.IsNullOrEmpty(rFor))
                rFor = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "SITE").ListTypeItemId;
            var site = context.Site.FirstOrDefault(x => x.Id == id);
            if (site == null) return null;

            var address = context.Address.FirstOrDefault(x => x.Parentid == id && x.Addressfor == rFor);
            var contacts = conObj.GetParentsContacts(id, rFor);

            var mSite = new SiteModel();
            var payTerms = context.Listtypeitems.FirstOrDefault(x => x.Id == site.Payterms);
            mSite.Distid = site.Distid;
            mSite.Custregname = site.Custregname;
            mSite.Id = site.Id;
            mSite.Isblocked = site.Isblocked;
            mSite.Isactive = site.Isactive;
            mSite.Payterms = site.Payterms;
            mSite.PaytermsValue = payTerms?.Itemname;
            mSite.Custid = site.Custid;
            mSite.Regname = site.Regname;

            mSite.Address = new AddressModel
            {
                Area = address.Area,
                City = address.City,
                Countryid = address.Countryid,
                CountryName = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == address.Countryid) != null
                    ? context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == address.Countryid).Itemname
                    : "",
                Geolat = address.Geolat,
                Geolong = address.Geolong,
                Place = address.Place,
                Street = address.Street,
                Zip = address.Zip,
                IsActive = address.Isactive
            };

            mSite.Contacts = contacts;

            return mSite;
        }
        private List<SiteModel> GetCustomerSites(string custId, string userId)
        {
            var commonMethods = new CommonMethods(context);
            var siteStrng = commonMethods.GetUserSitesByUserId(userId);
            var sitesList = siteStrng.Split(",");

            var sites = context.Site.Where(x => sitesList.Contains(x.Id) && x.Custid == custId).OrderBy(x => x.Custregname).ToList();
            var rFor = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "SITE").ListTypeItemId;

            return sites.Select(s => GetSite(s.Id, rFor)).ToList();
        }

        */


        public async Task<List<OfferRequestResponse>> GetAllOfferrequestAsync()
        {
            OfferRequestService offerRequestService = new OfferRequestService(context, currentUserService, configuration);
            return await offerRequestService.GetOfferRequestsAsync();           
        }

        /*
        private OfferrequestModel GetOfferrequest(Offerrequest offerrequest, string userId, string bUId, string brandId)
        {
            var commonMethods = new CommonMethods(context);
            var brands = commonMethods.GetBrandList(userId, brandId);
            var bUs = commonMethods.GetBusinessUnitList(userId, bUId);
            var lsInstruments = offerrequest.Instruments.Split(",");
            var hasInstrument = context.Instrument.Where(x => lsInstruments.Contains(x.Id) && brands.Contains(x.BrandId) && bUs.Contains(x.BusinessUnitId)).Any();
            if (!hasInstrument) return null;

            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "OFREQ" && x.UserName != "admin");
            var ofReq = context.Offerrequest.Where(x => !x.Isdeleted);
            var ofReqP = context.OfferRequestProcess.Where(x => !x.Isdeleted);

            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                ofReqP = ofReqP.Where(x => x.CreatedBy == userId);
                ofReq = ofReq.Where(x => x.Createdby == userId);
            }

            var oreq = ofReq.FirstOrDefault(x => x.Id == offerrequest.Id);
            if (oreq == null) return null;

            var custPrivilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "SCUST" && x.UserName != "admin");
            var cust = context.Customer.Where(x => !x.Isdeleted);

            if (custPrivilage != null && custPrivilage.PrivilageCode != "PARTS" && (custPrivilage._create || custPrivilage._read || custPrivilage._update || custPrivilage._delete))
            {
                cust = cust.Where(x => x.Createdby == userId);
            }

            var stage = "";
            var isCompleted = false;
            var isShipment = false;
            var stageLst = ofReqP.Where(x => x.OfferRequestId == offerrequest.Id && x.IsCompleted == true).OrderByDescending(x => x.UpdatedOn).ToList();
            var compDate = "";
            var compComments = "";
            decimal totalAmt = 0;

            if (stageLst.Count > 0)
            {
                stage = stageLst[0]?.Stage;
                stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == stage)?.Itemname;
                stage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stage);
                stage = stage.Replace('_', ' ');

                var compStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "COMPT" && x.ListCode == "OFRQP");

                OfferRequestProcess completedStage = null;
                OfferRequestProcess shipmntStage = null;

                var shipmentStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "SHDOC" && x.ListCode == "OFRQP");

                if (compStage != null)
                    completedStage = stageLst.FirstOrDefault(x => x.Stage == compStage.ListTypeItemId);

                if (compStage != null)
                    shipmntStage = stageLst.FirstOrDefault(x => x.Stage == shipmentStage.ListTypeItemId);

                if (completedStage != null)
                {
                    isCompleted = true;
                    compDate = completedStage.CreatedOn.ToString("MM/dd/yyyy");
                    compComments = completedStage.Comments;
                }

                isShipment = shipmntStage != null;

                foreach (var i in stageLst)
                {
                    totalAmt += Convert.ToDecimal(i.BaseCurrencyAmt * i.PayAmt);
                }

            }



            var mOfferrequest = new OfferrequestModel()
            {
                Id = offerrequest.Id,
                Createdby = offerrequest.Createdby,
                Createdon = offerrequest.Createdon,
                Updatedby = offerrequest.Updatedby,
                Updatedon = offerrequest.Updatedon,
                Isactive = offerrequest.Isactive,
                Distributorid = offerrequest.Distributorid,
                Distributor = context.Distributor.FirstOrDefault(x => x.Id == offerrequest.Distributorid).Distname,
                Totalamount = totalAmt,
                CustomerSiteId = offerrequest.CustomerSiteId,
                CurrencyId = offerrequest.CurrencyId,
                Status = offerrequest.Status,
                Podate = offerrequest.Podate,
                SpareQuoteNo = offerrequest.SpareQuoteNo,
                OtherSpareDesc = offerrequest.OtherSpareDesc,
                OffReqNo = offerrequest.OffReqNo,
                PaymentTerms = offerrequest.PaymentTerms,
                CustomerId = offerrequest?.CustomerId,
                CustomerName = context.Customer.FirstOrDefault(x => x.Id == offerrequest.CustomerId)?.Custname,
                InstrumentsList = offerrequest.Instruments,
                StageName = stage,
                IsCompleted = isCompleted,
                CompletedComments = compComments,
                CompletedDate = compDate,
                IsShipment = isShipment,
                CompanyId = offerrequest.CompanyId,

                InspectionChargesCurr = offerrequest.InspectionChargesCurr,
                InspectionChargesAmt = offerrequest.InspectionChargesAmt,
                AirFreightChargesAmt = offerrequest.AirFreightChargesAmt,
                AirFreightChargesCurr = offerrequest.AirFreightChargesCurr,
                LcadministrativeChargesAmt = offerrequest.LcadministrativeChargesAmt,
                LcadministrativeChargesCurr = offerrequest.LcadministrativeChargesCurr,
                BasePCurrencyAmt = offerrequest.BasePCurrencyAmt,
                TotalAmt = offerrequest.TotalAmt,
                TotalCurr = offerrequest.TotalCurr,

            };

            return mOfferrequest;
        }

        */

        public async Task<List<CustomerInstrumentResponse>> GetSiteInstrumentAsync(string siteId)
        {
            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

            CustomerInstrumentService customerInstrumentService = new CustomerInstrumentService(context, currentUserService, configuration);
            var custIns = await customerInstrumentService.GetAssignedCustomerInstrumentsAsync(string.Empty, string.Empty);
            return custIns.Where(x => x.CustSiteId.ToString() == siteId).ToList();

            /*
            var commonMethods = new CommonMethods(context,currentUserService);

            //var bUs = commonMethods.GetBusinessUnitList(userId, bUId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");

            var instr = await (from ci in context.CustomerInstrument
                               join i in context.Instrument on ci.InstrumentId equals i.Id
                               where ci.CustSiteId == siteId
                               seelct 

            if (privilage != null && privilage.PrivilageCode != "PARTS" &&
                (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                instr = instr.Where(x => x.Createdby == userId);
            }

            var instruments = instr.OrderBy(x => x.Serialnos).ToList();

            var resObj = instruments.Select(s => GetInstrumentMapped(s, userId, bUId, brandId))
                .Where(ins => ins != null).ToList();

            var result = new Message
            {
                Result = true,
                Object = resObj
            };

            return result;

            */
        }

        /*
        private InstrumentModel GetInstrumentMapped(Instrument instrument, string userId, string bUId, string brandId)
        {
            var commonMethods = new CommonMethods(context);
            var brands = commonMethods.GetBrandList(userId, brandId);
            var bUs = commonMethods.GetBusinessUnitList(userId, bUId);

            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var instr = context.Instrument.Where(x => !x.Isdeleted && bUs.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId));

            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                instr = instr.Where(x => x.Createdby == userId);
            }

            if (instr.FirstOrDefault(x => x.Id == instrument.Id) == null) return null;

            var mInstrument = new InstrumentModel
            {
                Id = instrument.Id,
                CustSiteId = instrument.Custsiteid,
                CustSiteName = context.Site.Any(x => x.Id == instrument.Custsiteid) ? context.Site.FirstOrDefault(x => x.Id == instrument.Custsiteid).Custregname : "",
                Engcontact = instrument.Engcontact,
                Engemail = instrument.Engemail,
                Engname = instrument.Engname,
                BusinessUnitId = instrument.BusinessUnitId,
                Insmfgdt = instrument.Insmfgdt,
                Installby = instrument.Installby,
                InstallbyOther = instrument.InstallbyOther,
                Installdt = instrument.Installdt,
                Instype = instrument.Instype,
                InstypeName = context.VW_ListItems.Where(x => x.ListTypeItemId == instrument.Instype)?.FirstOrDefault()?.Itemname,
                Insversion = instrument.Insversion,
                Serialnos = instrument.Serialnos,
                Shipdt = instrument.Shipdt,
                Warranty = instrument.Warranty,
                OperatorId = instrument.OperatorId,
                DateOfPurchase = instrument.DateOfPurchase,
                Cost = instrument.Cost,
                BrandId = instrument.BrandId,
                BaseCurrencyAmt = instrument.BaseCurrencyAmt,
                CurrencyId = instrument.CurrencyId,
                CustomerId = context.Site.FirstOrDefault(x => x.Id == instrument.Custsiteid)?.Custid,
                InstruEngineerId = instrument.InstruEngineerId
            };

            if (instrument.BusinessUnitId != null)
                mInstrument.BusinessUnit = context.BusinessUnit.FirstOrDefault(x => x.Id == instrument.BusinessUnitId)?.BusinessUnitName;
            if (mInstrument.BrandId != null)
                mInstrument.Brand = context.Brand.FirstOrDefault(x => x.Id == instrument.BrandId)?.BrandName;
            if (!instrument.Warranty) return mInstrument;

            mInstrument.Wrntyendt = instrument.Wrntyendt;
            mInstrument.Wrntystdt = instrument.Wrntystdt;

            return mInstrument;
        }
        */

        public async Task<List<CustomerInstrumentResponse>> GetSerReqInstrumentAsync(string instrId)
        {

            CustomerInstrumentService customerInstrumentService = new CustomerInstrumentService(context, currentUserService, configuration);
            var custIns = await customerInstrumentService.GetAssignedCustomerInstrumentsAsync(string.Empty, string.Empty);
            return custIns.Where(x => x.InstrumentId.ToString() == instrId).ToList();

            /*
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var cDl = new ContactDL(context);
            var srInstr = new SRInstrumentModel();

            var commonMethods = new CommonMethods(context);

            var sites = commonMethods.GetUserSitesByUserId(userId);
            var lstSites = sites.Split(",");

            var privilage = context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "CUSDH" && x.UserName != "admin");
            var inst = context.Instrument.Where(x => !x.Isdeleted);

            if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            {
                inst = inst.Where(x => x.Createdby == userId);
            }


            var instr = inst.FirstOrDefault(x => x.Id == instrId);
            if (!lstSites.Contains(instr.Custsiteid)) return null;

            srInstr.CustSiteId = instr.Custsiteid;
            srInstr.Id = instr.Id;
            srInstr.BusinessUnitId = instr.BusinessUnitId;
            srInstr.InstruEngineerId = instr.InstruEngineerId;
            srInstr.Instype = context.VW_ListItems.Where(x => x.ListTypeItemId == instr.Instype)?.FirstOrDefault()?.Itemname; // instr.Instype;
            srInstr.Insversion = instr.Insversion;
            srInstr.OperatorId = instr.OperatorId;
            srInstr.Serialnos = instr.Serialnos;
            srInstr.MachineEng = cDl.GetContact(instr.InstruEngineerId);
            srInstr.OperatorEng = cDl.GetContact(instr.OperatorId);
            srInstr.BrandId = instr.BrandId;

            var result = new Message
            {
                Result = false,
                Object = srInstr
            };

            return result;

            */
        }
    }
}
