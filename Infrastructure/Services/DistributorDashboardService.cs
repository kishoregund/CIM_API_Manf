using Application.Features.AMCS.Responses;
using Application.Features.AppBasic.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;
using Application.Features.Identity.Users;
using Application.Features.ServiceRequests.Responses;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.OpenApi;
using Infrastructure.Persistence.Contexts;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace Infrastructure.Services
{
    public class DistributorDashboardService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : IDistributorDashboardService
    {
        public async Task<DistDashboardSerReqModel> GetDistDashboardDataAsync(DashboardDateRequest dashboardDate)
        {
            var commonMethods = new CommonMethods(context, currentUserService, configuration);
            var distDashboard = new DistDashboardSerReqModel();

            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

                var lstSites = await commonMethods.GetSitesByUserIdAsync();
                var lstRegionsprofile = await commonMethods.GetDistRegionsByUserIdAsync();

                var instruments = context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId).ToList();

                //var serviceRequest = new List<ServiceRequest>();
                int serviceRequestCnt = 0;
                var serReq = (from a in context.ServiceRequest.ToList()
                              join b in instruments on Guid.Parse(a.MachinesNo) equals b.Id
                              where a.DistId == userProfile.EntityParentId && lstSites.Contains(a.SiteId.ToString())
                              select new
                              {
                                  b.SerialNos,
                                  a.Id,
                                  a.SerReqNo,
                                  a.DistId,
                                  a.CustId,
                                  a.AssignedTo,
                                  a.IsReportGenerated,
                                  a.CreatedOn
                              }).ToList();

                foreach (var item in serReq)
                {
                    dashboardDate.CreatedOn = item.CreatedOn;

                    if (commonMethods.GetDateDiff(dashboardDate))
                        serviceRequestCnt++;
                }
                //serReq = serviceRequest;

                var insHSerReq = serReq.GroupBy(x => x.SerialNos)
                    .Select(x => new { key = x.Key, count = x.Count() }).OrderByDescending(x => x.count).ToList();

                #region commented
                //var insDetails = (from i in instruments //context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId).ToList()
                //                  join ci in context.CustomerInstrument on i.Id equals ci.InstrumentId
                //                  join site in context.Site on ci.CustSiteId equals site.Id
                //                  //join c in context.Customer on site.CustomerId equals c.Id

                //                  select new
                //                  {
                //                      CustSiteId = site.Id,
                //                      RegionId = site.RegionId,
                //                      Id = i.Id,
                //                      SerialNos = i.SerialNos
                //                      //BaseCurrencyAmt = ci.BaseCurrencyAmt,
                //                      //InstrumentId = ci.InstrumentId,
                //                      //Cost = ci.Cost,
                //                      //CurrencyId = ci.CurrencyId,
                //                      //CreatedBy = ci.CreatedBy,
                //                      //CreatedOn = ci.CreatedOn,                                      
                //                      //CustSiteName = site.CustRegName,                                      
                //                      //DateOfPurchase = ci.DateOfPurchase,
                //                      //EngContact = ci.EngContact,
                //                      //EngEmail = ci.EngEmail,
                //                      //EngName = ci.EngName,
                //                      //EngNameOther = ci.EngNameOther,

                //                      //InsMfgDt = ci.InsMfgDt,
                //                      //InstallBy = ci.InstallBy,
                //                      //InstallByName = context.Distributor.FirstOrDefault(x => x.Id == site.DistId).DistName,
                //                      //InstallByOther = ci.InstallByOther,
                //                      //InstallDt = ci.InstallDt,
                //                      //InstruEngineerId = ci.InstruEngineerId,
                //                      //InstrumentSerNoType = context.ListTypeItems.FirstOrDefault(x => x.Id.ToString() == i.InsType).ItemName + " - " + i.SerialNos,
                //                      //IsActive = ci.IsActive,
                //                      //IsDeleted = ci.IsDeleted,
                //                      //OperatorId = ci.OperatorId,
                //                      //ShipDt = ci.ShipDt,
                //                      //Warranty = ci.Warranty,
                //                      //WrntyEnDt = ci.WrntyEnDt,
                //                      //WrntyStDt = ci.WrntyStDt,
                //                      //InsType = i.InsType,
                //                      //InsTypeName = context.ListTypeItems.FirstOrDefault(x => x.Id.ToString() == i.InsType).ItemName,
                //                      //InsVersion = i.InsVersion,
                //                      //ManufId = i.ManufId,

                //                  }).ToList();

                //var ins = (from a in insHSerReq
                //           join b in insDetails.Where(x => lstSites.Contains(x.CustSiteId.ToString()) && lstRegionsprofile.Contains(x.RegionId.ToString()))
                //           on a.key equals b.Id.ToString()
                //           select new { a.count, key = b.SerialNos }).ToList();

                #endregion

                var engHSerReq = (from s in context.ServiceRequest.Where(x=> lstSites.Contains(x.SiteId.ToString()))
                                  join rc in context.RegionContact on s.AssignedTo equals rc.Id
                                  join site in context.Site on s.SiteId equals site.Id
                                  join c in context.Customer on s.CustId equals c.Id
                                  where s.DistId == userProfile.EntityParentId  && !s.IsReportGenerated
                                  select new DashboardDataResponse()
                                  {
                                      CustId = c.Id.ToString(),
                                      CustName = c.CustName,
                                      EngName = rc.FirstName + ' ' + rc.LastName,
                                      EngId = rc.Id.ToString(),
                                      DistId = s.DistId.ToString(),
                                      Createdon = s.CreatedOn,
                                      SiteRegion = site.DistId.ToString()
                                  }).ToList();

                var engH = new List<DashboardDataResponse>();
                foreach (var item in engHSerReq)
                {
                    dashboardDate.CreatedOn = item.Createdon;
                    if (commonMethods.GetDateDiff(dashboardDate)) engH.Add(item);
                }

                engHSerReq = engH;

                distDashboard.EngHandlingReq = new List<EngHandlingSReq>();
                distDashboard.InstrumentWithHighestServiceRequest = insHSerReq.Take(5); // ins.Take(5);
                distDashboard.ServiceRequestRaised = serReq.ToList().Count();

                foreach (var x in engHSerReq)
                {
                    var engHandlingS = new EngHandlingSReq
                    {
                        CustId = x.CustId,
                        CustTotalSReq = serReq.Where(sreq =>
                            sreq.CustId == Guid.Parse(x.CustId) && sreq.DistId == userProfile.EntityParentId &&
                            sreq.IsReportGenerated == false).ToList().Count(),
                        CustName = x.CustName,
                        EngName = x.EngName,
                        EngId = x.EngId,
                        EngAssignedToCust = serReq.Where(req =>
                            req.CustId.ToString().ToUpper() == x.CustId.ToUpper() && req.AssignedTo.ToString().ToUpper() == x.EngId.ToUpper() && req.DistId == userProfile.EntityParentId &&
                            req.IsReportGenerated == false).ToList().Count()
                    };

                    var perCent = Math.Round((engHandlingS.EngAssignedToCust / engHandlingS.CustTotalSReq) * 100, 2);
                    engHandlingS.EngCustPercent = 0.00;

                    if (!double.IsNaN(perCent) && !double.IsInfinity(perCent)) engHandlingS.EngCustPercent = perCent;
                    if (distDashboard.EngHandlingReq.FirstOrDefault(y => y.EngId == x.EngId && y.CustId == x.CustId) == null)
                        distDashboard.EngHandlingReq.Add(engHandlingS);
                }

                distDashboard.EngHandlingReq.OrderByDescending(x => x.EngCustPercent).GroupBy(x => x.CustId);

            }
            catch (Exception ex)
            { }
            return distDashboard;
        }

        public async Task<List<ServiceRequestRaisedResponse>> GetEngHandlingServiceRequest(string businessUnitId, string brandId)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

            //var distId = GetDistIdByUserId(userId);
            //var result = new Message();
            //var commonMethods = new CommonMethods(context);
            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var buId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var bUs = commonMethods.GetBusinessUnitList(userId, buId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "SRREQ" && x.UserName != "admin");
            var instruments = (from a in context.ServiceRequest.Where(x => !x.IsDeleted)
                               join b in context.Instrument.Where(x => x.BusinessUnitId.ToString() == businessUnitId && x.BrandId.ToString() == brandId)
                                   on a.MachinesNo equals b.Id.ToString()
                               where a.DistId == userProfile.EntityParentId
                               select new ServiceRequest
                               {
                                   CreatedOn = a.CreatedOn,
                                   AssignedTo = a.AssignedTo,
                                   Id = a.Id
                               }).OrderByDescending(x => x.CreatedOn).ToList();

            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    serReq = serReq.Where(x => x.Createdby == userId);

            //var instruments = serReq.Where(x => x.Distid == distId).OrderByDescending(x => x.Createdon)
            //    .Select(x => new ServiceRequest
            //    {
            //        Companyname = x.Companyname,
            //        Createdon = x.Createdon,
            //        Assignedto = x.Assignedto,
            //        Id = x.Id
            //    }).ToList();

            var instrumentsGroup = from instrument in instruments
                                   group instrument by new
                                   {
                                       instrument.AssignedTo,
                                   };

            var lstCountServiceRequest = new List<ServiceRequestRaisedResponse>();

            foreach (var group in instrumentsGroup)
            {

                var key = group.Key;

                var count = group.Count();
                var countServiceRequest = new ServiceRequestRaisedResponse
                {
                    Count = count,
                    Object = key
                };

                lstCountServiceRequest.Add(countServiceRequest);
            }

            return lstCountServiceRequest;
        }

        public async Task<List<ServiceRequestRaisedResponse>> GetInstByHighestServiceRequest(string businessUnitId, string brandId)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            //var distId = GetDistIdByUserId(userId);
            //var result = new Message();
            //var commonMethods = new CommonMethods(context);

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var buId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var bUs = commonMethods.GetBusinessUnitList(userId, buId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "SRREQ" && x.UserName != "admin");
            //var serReq = (from a in context.ServiceRequest.Where(x => !x.Isdeleted)
            //              join b in context.Instrument.Where(x => bUs.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId))
            //                  on a.Machinesno equals b.Id
            //              select a);

            var instruments = (from a in context.ServiceRequest.Where(x => !x.IsDeleted)
                               join b in context.Instrument.Where(x => x.BusinessUnitId.ToString() == businessUnitId && x.BrandId.ToString() == brandId)
                                   on a.MachinesNo equals b.Id.ToString()
                               where a.DistId == userProfile.EntityParentId
                               select a).OrderByDescending(x => x.CreatedOn).ToList();

            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    serReq = serReq.Where(x => x.Createdby == userId);


            //var instruments = serReq.Where(x => x.Distid == distId)
            //    .OrderByDescending(x => x.Createdon).ToList();

            var instrumentsGroup = from instrument in instruments
                                   group instrument by instrument.MachinesNo;

            var countservicerequest = new List<ServiceRequestRaisedResponse>();

            foreach (var group in instrumentsGroup)
            {
                var key = group.Key;
                var grp = new ServiceRequestRaisedResponse
                {
                    Object = key,
                    Count = group.Count()
                };
                countservicerequest.Add(grp);
            }

            return countservicerequest;
        }

        public async Task<object> GetInstrumentInstalled(DashboardDateRequest dashboardDate)
        {
            var commonMethods = new CommonMethods(context, currentUserService, configuration);
            var lstRegionsprofile = await commonMethods.GetDistRegionsByUserIdAsync();

            var instruments = context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId).ToList();
            var sites = context.Site.Where(x => lstRegionsprofile.Contains(x.RegionId.ToString())).ToList();
            var iInstruments = (from i in instruments
                                join ci in context.CustomerInstrument on i.Id equals ci.InstrumentId
                                join s in sites on ci.CustSiteId equals s.Id
                                select ci).ToList();

            var installedInstruments = 0;
            foreach (var ins in iInstruments)
            {
                dashboardDate.CreatedOn = ins.CreatedOn;
                if (!commonMethods.GetDateDiff(dashboardDate)) continue;

                if (context.CustomerInstrument.Any(x => x.InstrumentId == ins.InstrumentId))
                    installedInstruments++;

            }

            var iUnderService = (from i in instruments
                                 join ai in context.AMCInstrument on i.Id equals ai.InstrumentId
                                 join a in context.AMC on ai.AMCId equals a.Id
                                 join site in sites on a.CustSite equals site.Id
                                 select ai).ToList();

            var instrumentsUnderService = 0;
            foreach (var ins in iUnderService)
            {
                dashboardDate.CreatedOn = ins.CreatedOn;
                if (!commonMethods.GetDateDiff(dashboardDate)) continue;

                if (context.AMCInstrument.Any(x => x.InstrumentId == ins.InstrumentId))
                    instrumentsUnderService++;

            }

            return new { installedInstruments, instrumentsUnderService };
        }

        public async Task<List<ServiceRequestRaisedResponse>> GetInstrumentsInstalled(string businessUnitId, string brandId)
        {
            //var distId = GetDistIdByUserId(userId);
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());


            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var result = new Message();
            //var commonMethods = new CommonMethods(context);
            //var buId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var bUs = commonMethods.GetBusinessUnitList(userId, buId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "SINST" && x.UserName != "admin");
            var instruments = (from i in context.Instrument.Where(x => !x.IsDeleted && x.BrandId.ToString() == brandId && x.BusinessUnitId.ToString() == businessUnitId).ToList()
                               join ci in context.CustomerInstrument on i.Id equals ci.InstrumentId
                               where ci.InstallBy == userProfile.EntityParentId.ToString()
                               select i).OrderByDescending(x => x.CreatedOn).ToList();

            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    instr = instr.Where(x => x.Createdby == userId);

            //var instruments = instr.Where(x => x.Installby == distId).OrderByDescending(x => x.Createdon).ToList();

            var instrumentsGroup = from instrument in instruments group instrument by instrument.CreatedOn;

            var countservicerequest = new List<ServiceRequestRaisedResponse>();

            foreach (var group in instrumentsGroup)
            {
                var requestRaised = new ServiceRequestRaisedResponse
                {
                    Date = group.Key,
                    Count = group.Count()
                };
                countservicerequest.Add(requestRaised);
            }

            return countservicerequest;
        }

        public async Task<List<ServiceRequestRaisedResponse>> GetServiceRequestRaised(string businessUnitId, string brandId)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

            //var distId = GetDistIdByUserId(userId);

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var result = new Message();
            //var commonMethods = new CommonMethods(context);
            //var buId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var bUs = commonMethods.GetBusinessUnitList(userId, buId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            //var privilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "SRREQ" && x.UserName != "admin");
            var serReqs = (from a in context.ServiceRequest.Where(x => !x.IsDeleted)
                           join b in context.Instrument.Where(x => x.BusinessUnitId.ToString() == businessUnitId && x.BrandId.ToString() == brandId)
                               on a.MachinesNo equals b.Id.ToString()
                           where a.DistId == userProfile.EntityParentId
                           select a).OrderByDescending(x => x.CreatedOn).ToList();

            //if (privilage != null && privilage.PrivilageCode != "PARTS" &&
            //    (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    serReq = serReq.Where(x => x.Createdby == userId);

            //var serviceRequests = serReqs.Where(x => x.Distid == distId).OrderByDescending(x => x.Createdon).ToList();
            var serviceRequestsGroup = from serviceRequest in serReqs group serviceRequest by serviceRequest.CreatedOn;

            var countservicerequest = new List<ServiceRequestRaisedResponse>();

            foreach (var group in serviceRequestsGroup)
            {
                var requestRaised = new ServiceRequestRaisedResponse
                {
                    Date = group.Key,
                    Count = group.Count()
                };
                countservicerequest.Add(requestRaised);
            }

            return countservicerequest;
        }

        public async Task<object> GetRevenueFromCustomer(DashboardDateRequest dashboardDate)
        {
            var commonMethods = new CommonMethods(context, currentUserService, configuration);
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            //var result = new Message();

            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            //var buId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
            //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
            //var bUs = commonMethods.GetBusinessUnitList(userId, buId);
            //var brands = commonMethods.GetBrandList(userId, brandId);

            var lstRevenue = new List<CustomerRevenueResponse>();
            var lstRegionsprofile = commonMethods.GetDistRegionsByUserIdAsync().Result;

            //var amcprivilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "DISDH" && x.UserName != "admin");

            var amc = from a in context.AMC.Where(x => !x.IsDeleted)
                      join b in context.AMCInstrument.Where(x => !x.IsDeleted) on a.Id equals b.AMCId
                      join c in context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId)
                          on b.InstrumentId equals c.Id
                      select a;

            //if (amcprivilage != null && amcprivilage.PrivilageCode != "PARTS" && (amcprivilage._create ||
            //        amcprivilage._read || amcprivilage._update || amcprivilage._delete))
            //    amc = amc.Where(x => x.Createdby == userId);

            //var custprivilage = context.Vw_Privilages.FirstOrDefault(x =>
            //    x.UserId == userId && x.ScreenCode == "DISDH" && x.UserName != "admin");
            var cust = context.Customer.Where(x => !x.IsDeleted);

            //if (custprivilage != null && custprivilage.PrivilageCode != "PARTS" && (custprivilage._create ||
            //        custprivilage._read || custprivilage._update || custprivilage._delete))
            //    cust = cust.Where(x => x.Createdby == userId);

            var allAmc = amc.ToList();
            var customers = cust.Where(x => lstRegionsprofile.Contains(x.DefDistRegionId.ToString())).ToList();
            var sites = context.Site.ToList();
            var instruments = (from i in context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId).ToList()
                               join ci in context.CustomerInstrument on i.Id equals ci.Id
                               select ci).ToList();
            var spareParts = (from s in context.Spareparts
                              join insp in context.InstrumentSpares on s.Id equals insp.SparepartId
                              select new
                              {
                                  s.Id,
                                  s.Qty,
                                  s.Price,
                                  s.CreatedOn,
                                  insp.InstrumentId
                              }).ToList();

            foreach (var customer in customers)
            {
                var revenue = new CustomerRevenueResponse
                {
                    Customer = customer,
                    Total = 0
                };

                foreach (var aMC in allAmc.Where(x => x.BillTo == customer.Id).ToList())
                {
                    dashboardDate.CreatedOn = aMC.CreatedOn;

                    if (commonMethods.GetDateDiff(dashboardDate)) revenue.Total += aMC.Zerorate;
                }

                foreach (var ins in sites.Where(x => x.CustomerId == customer.Id).ToList()
                             .SelectMany(site => instruments.Where(x => x.CustSiteId == site.Id).ToList()))
                {
                    dashboardDate.CreatedOn = ins.CreatedOn;
                    if (commonMethods.GetDateDiff(dashboardDate))
                        revenue.Total += ins.Cost.HasValue
                            ? (decimal)(ins.Cost.Value * ins.BaseCurrencyAmt.Value)
                            : 0;

                    // revist this sparepart logic as its calculating all

                    foreach (var sp in spareParts.Where(x => x.InstrumentId == ins.Id).ToList())
                    {
                        dashboardDate.CreatedOn = sp.CreatedOn;
                        if (commonMethods.GetDateDiff(dashboardDate))
                            revenue.Total += sp.Price * (ins.BaseCurrencyAmt > 0 ? (decimal)ins.BaseCurrencyAmt : 0);
                    }
                }

                lstRevenue.Add(revenue);
            }

            return lstRevenue.OrderByDescending(x => x.Total).Take(5); ;
        }

        public async Task<ServiceContractRevenueResponse> GetServiceContractRevenue(DashboardDateRequest dashboardDate)
        {
            var commonMethods = new CommonMethods(context, currentUserService, configuration);
            var lstRegionsprofile = commonMethods.GetDistRegionsByUserIdAsync().Result;

            decimal? amcRevenue = 0;
            decimal? breakdownRevenue = 0;
            decimal? preventiveRevenue = 0;
            decimal? oncallRevenue = 0;
            decimal? plannedRevenue = 0;

            var Amc = await (from a in context.AMC
                             join b in context.AMCInstrument on a.Id equals b.AMCId
                             join c in context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId)
                                 on b.InstrumentId equals c.Id
                             select a).ToListAsync();
            var cust = context.Customer;

            var allAmc = Amc.ToList();
            var customers = cust.Where(x => lstRegionsprofile.Contains(x.DefDistRegionId.ToString())).ToList();

            var lstAmc = allAmc.Where(a => customers.Select(x => x.Id).Contains(a.BillTo)).ToList();
            var mAmc = (from amc in lstAmc select GetAMC(amc.Id, currentUserService.GetUserId()))
                .ToList();

            var grpAmc = new List<AMCResponse>();

            foreach (var amc in mAmc)
            {
                dashboardDate.CreatedOn = amc.CreatedOn;

                if (!commonMethods.GetDateDiff(dashboardDate)) continue;

                amcRevenue += amc.Zerorate * amc.BaseCurrencyAmt;

                grpAmc.Add(amc);
            }

            var serviceRequests = (from sr in context.ServiceRequest
                                   join i in context.Instrument.Where(x => x.BusinessUnitId == dashboardDate.BusinessUnitId && x.BrandId == dashboardDate.BrandId)
                                   on sr.MachinesNo equals i.Id.ToString()
                                   select sr).ToList().Adapt<List<ServiceRequestResponse>>();



            foreach (ServiceRequestResponse sr in serviceRequests)
            {
                var visitType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Guid.Parse(sr.VisitType)).ItemCode;
                switch (visitType)
                {
                    case "PREV":
                        preventiveRevenue += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "BRKDW":
                        breakdownRevenue += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "ONCAL":
                        oncallRevenue += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;

                    case "PLAN":
                        plannedRevenue += sr.TotalCost != null ? sr.TotalCost : 0;
                        break;
                }
            }

            return new ServiceContractRevenueResponse
            {
                AmcRevenue = amcRevenue,
                PreventiveRevenue = preventiveRevenue,
                BreakdownRevenue = breakdownRevenue,
                OncallRevenue = oncallRevenue,
                PlannedRevenue = plannedRevenue,
                GrpAmc = grpAmc
            };

        }

        private AMCResponse GetAMC(Guid id, string userId)
        {

            var Amc = context.AMC.Where(x => x.Id == id).OrderBy(x => x.Project).FirstOrDefault();
            if (Amc == null) return null;

            var isCompleted = false;
            var stage = "";
            var stageLst = context.AMCStages.Where(x => x.AMCId == Amc.Id && x.IsCompleted == true)
                ?.OrderByDescending(x => x.UpdatedOn).ToList();

            if (stageLst.Count > 0)
            {
                stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == stageLst[0].Stage)?.ItemName;
                stage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stage);

                var compStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "COMPT" && x.ListCode == "AMCSG");
                var completedStage = stageLst.FirstOrDefault(x => x.Stage == compStage.ListTypeItemId.ToString());
                isCompleted = completedStage != null;

            }

            return new AMCResponse
            {
                Id = Amc.Id,
                IsActive = Amc.IsActive,
                CreatedOn = Amc.CreatedOn,
                CustSite = Amc.CustSite,
                CustSiteName = context.Site.FirstOrDefault(x => x.Id == Amc.CustSite)?.CustRegName,
                BillTo = Amc.BillTo,
                BillToName = context.Customer.FirstOrDefault(x => x.Id == Amc.BillTo)?.CustName,
                ServiceQuote = Amc.ServiceQuote,
                SqDate = Amc.SqDate,
                EDate = Amc.EDate,
                SDate = Amc.SDate,
                Project = Amc.Project,
                ServiceType = Amc.ServiceType,
                BrandId = Amc.BrandId,
                CurrencyId = Amc.CurrencyId,
                Zerorate = Amc.Zerorate,
                BaseCurrencyAmt = Amc.BaseCurrencyAmt,
                TnC = Amc.TnC,
                StageName = stage,
                PaymentTerms = Amc.PaymentTerms,
                SecondVisitDate = Amc.SecondVisitDate,
                FirstVisitDate = Amc.FirstVisitDate,
                IsCompleted = isCompleted,
                AMCServiceType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == Amc.ServiceType)?.ItemName,
                AMCServiceTypeCode = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == Amc.ServiceType)?.ItemCode
            };
        }

    }
}
