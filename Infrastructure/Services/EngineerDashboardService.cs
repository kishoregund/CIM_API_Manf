using Application.Features.AMCS.Responses;
using Application.Features.AppBasic.Responses;
using Application.Features.Customers.Responses;
using Application.Features.Dashboards;
using Application.Features.Dashboards.Requests;
using Application.Features.Dashboards.Responses;
using Application.Features.Identity.Users;
using Application.Features.Manufacturers.Queries;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace Infrastructure.Services
{
    public class EngineerDashboardService(ApplicationDbContext context, ICurrentUserService currentUserService) : IEngineerDashboardService
    {
        public async Task<List<EngServiceRequestResponse>> GetServiceRequestAsync(string date)
        {
            var lstSerReq = new List<EngServiceRequestResponse>();
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //var user = _context.Users.FirstOrDefault(x => x.Id == userId);

                //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
                //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
                //var commonMethods = new CommonMethods(_context);
                //var bus = commonMethods.GetBusinessUnitList(userId, bUId);
                //var brands = commonMethods.GetBrandList(userId, brandId);

                var bus = userProfile.BusinessUnitIds.Split(',');
                var brands = userProfile.BrandIds.Split(',');

                var engSerReq = await (from s in context.ServiceRequest
                                       join rc in context.RegionContact on s.AssignedTo equals rc.Id
                                       join li in context.VW_ListItems on s.VisitType equals li.ListTypeItemId.ToString()
                                       join i in context.Instrument.Where(x => bus.Contains(x.BusinessUnitId.ToString()) && brands.Contains(x.BrandId.ToString()))
                                       on s.MachinesNo equals i.Id.ToString()
                                       where s.AssignedTo == userProfile.ContactId
                                       select new EngServiceRequestResponse()
                                       {
                                           ContactId = rc.Id.ToString(),
                                           Createdby = s.CreatedBy.ToString(),
                                           CreatedOn = s.CreatedOn,
                                           IsDeleted = s.IsDeleted,
                                           IsReportGenerated = s.IsReportGenerated,
                                           MachinesNo = s.MachinesNo,
                                           SerReqNo = s.SerReqNo,
                                           ServiceType = li.ItemName,
                                           ServiceTypeCode = li.ItemCode,
                                           ServiceTypeId = s.VisitType,
                                           UserName = rc.FirstName + ' ' + rc.LastName
                                       }).ToListAsync();


                //var engSerReq = (from a in _context.Vw_EngServiceRequest.Where(x => !x.IsDeleted)
                //join b in _context.Instrument.Where(x => bus.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId)) on a.MachineSno equals b.Id select a);
                //var privilage = _context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "SRREQ" && x.UserName != "admin");
                //if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
                //    engSerReq = engSerReq.Where(x => x.Createdby == userId);

                //var serReq = engSerReq.Where(x => x.ContactId == user.Contactid).ToList();

                foreach (var item in engSerReq)
                {
                    if (GetDateDiff(item.CreatedOn, DateTime.Now, date)) lstSerReq.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // Variable is declared but never used

            return lstSerReq;
        }

        public async Task<List<VW_SparesRecommended>> GetSparesRecommendedAsync(string date)
        {
            var lstSerReq = new List<VW_SparesRecommended>();

#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
                //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;

                //var commonMethods = new CommonMethods(_context);
                //var bus = commonMethods.GetBusinessUnitList(userId, bUId);
                //var brands = commonMethods.GetBrandList(userId, brandId);

                var bus = userProfile.BusinessUnitIds.Split(',');
                var brands = userProfile.BrandIds.Split(',');

                var spr = context.VW_SparesRecommended.Where(x => !x.IsDeleted && bus.Contains(x.BusinessUnitId.ToString()) && brands.Contains(x.BrandId.ToString()));
                //var privilage = _context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "SPRCM" && x.UserName != "admin");
                //if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
                //    spr = spr.Where(x => x.Createdby == userId);


                var serReq = spr.Where(x => x.AssignedToId == userProfile.ContactId).ToList();

                foreach (var item in serReq)
                {
                    if (GetDateDiff(item.CreatedOn, DateTime.Now, date)) lstSerReq.Add(item);
                }


            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // Variable is declared but never used
            return lstSerReq;
        }

        public async Task<List<SparesConsumedResponse>> GetSparesConsumedAsync(string date)
        {
            var lstSerReq = new List<SparesConsumedResponse>();
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

                //var claimsIdentity = this.User.Identity as ClaimsIdentity;
                //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                //var bUId = claimsIdentity.FindFirst(ClaimTypes.Country)?.Value;
                //var brandId = claimsIdentity.FindFirst(ClaimTypes.GivenName)?.Value;
                //var commonMethods = new CommonMethods(_context);
                //var bus = commonMethods.GetBusinessUnitList(userId, bUId);
                //var brands = commonMethods.GetBrandList(userId, brandId);

                var bus = userProfile.BusinessUnitIds.Split(',');
                var brands = userProfile.BrandIds.Split(',');

                var spConsumed = (from sc in context.SPConsumed
                                  join srp in context.ServiceReport on sc.ServiceReportId equals srp.Id
                                  join sr in context.ServiceRequest on srp.ServiceRequestId equals sr.Id
                                  join i in context.Instrument.Where(x => bus.Contains(x.BusinessUnitId.ToString()) && brands.Contains(x.BrandId.ToString()))
                                  on sr.MachinesNo equals i.Id.ToString()
                                  where sr.AssignedTo == userProfile.ContactId
                                  select new SparesConsumedResponse()
                                  {
                                      AssignedTo = sr.AssignedTo.ToString(),
                                      BrandId = i.BrandId.ToString(),
                                      BusinessUnitId = i.BusinessUnitId.ToString(),
                                      CreatedOn = srp.CreatedOn,
                                      IsDeleted = srp.IsDeleted,
                                      PartNo = sc.PartNo,
                                      QtyConsumed = sc.QtyConsumed,
                                      SerReqNo = sr.SerReqNo
                                  }).ToList();

                //var serReq = _context.Vw_SPConsumed.Where(x => x.AssignedTo == user.Contactid && bus.Contains(x.BusinessUnitId) && brands.Contains(x.BrandId)).ToList();




                foreach (var item in spConsumed)
                {
                    if (GetDateDiff(item.CreatedOn, DateTime.Now, date)) lstSerReq.Add(item);
                }

            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // Variable is declared but never used
            return lstSerReq;
        }

        public async Task<object> GetTravelExpensesAsync(string date)
        {

            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            //var privilage = _context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "TREXP" && x.UserName != "admin");
            var te = context.TravelExpenses.Where(x => !x.IsDeleted);
            //if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            //    te = te.Where(x => x.Createdby == userId);

            //var advPrivilage = _context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "TREXP" && x.UserName != "admin");
            var advance = context.AdvanceRequest.Where(x => !x.IsDeleted && x.EngineerId == userProfile.ContactId).ToList();
            //if (advPrivilage != null && advPrivilage.PrivilageCode != "PARTS" && (advPrivilage._create || advPrivilage._read || advPrivilage._update || advPrivilage._delete))
            //    adv = adv.Where(x => x.CreatedBy == userId);

            var expenseItems = (from a in context.TravelExpenses.Where(x => x.EngineerId == userProfile.ContactId)
                                join b in context.TravelExpenseItems on a.Id equals b.TravelExpenseId
                                join c in context.VW_ListItems on b.ExpNature equals c.ListTypeItemId
                                select new { b, c }).ToList();
            //var advance = adv.Where(x => x.EngineerId == advPrivilage.ContactId).ToList();

            decimal advanceRequest = 0;
            decimal visaRelated = 0;
            decimal others = 0;
            decimal hotel = 0;
            decimal da = 0;
            decimal airTicket = 0;
            decimal localTravel = 0;
            decimal total = 0;

            foreach (var item in expenseItems)
            {
                total += item.b.UsdAmt;

                switch (item.c.ItemCode)
                {
                    case "EXVAR":
                        visaRelated += item.b.UsdAmt;
                        break;

                    case "EXOTR":
                        others += item.b.UsdAmt;
                        break;

                    case "EXHTL":
                        hotel += item.b.UsdAmt;
                        break;

                    case "EXPDA":
                        da += item.b.UsdAmt;
                        break;

                    case "EXATK":
                        airTicket += item.b.UsdAmt;
                        break;

                    case "EXLCT":
                        localTravel += item.b.UsdAmt;
                        break;
                }


            }

            foreach (var ad in advance) advanceRequest += ad.AdvanceAmount;

            total += advanceRequest;

            return new { localTravel, airTicket, da, hotel, others, visaRelated, total, advanceRequest };

        }

        public bool GetDateDiff(DateTime sDate, DateTime eDate, string type)
        {
            var isValidDate = eDate > sDate;
            var diff = (eDate - sDate).TotalDays;
            if (isValidDate)
            {
                if (type == "3MNTHS" && diff <= 90 && diff >= 0) return true;
                else if (type == "6MNTHS" && diff <= 180 && diff >= 0) return true;
                else if (type == "12MNTHS" && diff <= 365 && diff >= 0) return true;
            }

            return false;
        }

    }
}
