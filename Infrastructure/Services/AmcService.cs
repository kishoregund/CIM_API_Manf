using Application.Features.AMCS;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Features.AMCS.Responses;
using System.Globalization;
using Application.Features.Dashboards.Responses;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class AmcService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAmcService
    {
        public async Task<Guid> CreateAmc(AMC amc)
        {
            amc.CreatedOn = DateTime.Now;
            amc.UpdatedOn = DateTime.Now;
            amc.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            amc.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            if ((DateTime.Now.Date < DateTime.ParseExact(amc.EDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date))
            {
                amc.IsActive = true;
            }
            else { amc.IsActive = false; }

            amc.IsDeleted = false;
            await context.AMC.AddAsync(amc);
            await context.SaveChangesAsync();
            return amc.Id;
        }

        public async Task<bool> DeleteAmc(Guid id)
        {

            var deleteAmc = await context.AMC.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (deleteAmc == null) return true;

            deleteAmc.IsDeleted = true;
            deleteAmc.IsActive = false;

            context.Entry(deleteAmc).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AmcResponse>> GetAllAsync()
        {
            var userProfile = context.VW_UserProfile.FirstOrDefault(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            List<AmcResponse> lstAMCs = new List<AmcResponse>();
            if (userProfile.ContactType == "DR")
            {
                var regions = userProfile.DistRegions.Split(',');
                lstAMCs=  await (from a in context.AMC
                              join s in context.Site on a.CustSite equals s.Id
                              where regions.Contains(s.RegionId.ToString())
                              select new AmcResponse
                              {
                                  BaseCurrencyAmt = a.BaseCurrencyAmt,
                                  BillTo = a.BillTo,
                                  BrandId = a.BrandId,
                                  ConversionAmount = a.ConversionAmount,
                                  CurrencyId = a.CurrencyId,
                                  CustSite = a.CustSite,
                                  EDate = a.EDate,
                                  FirstVisitDate = a.FirstVisitDate,
                                  Id = a.Id,
                                  IsActive = a.IsActive,
                                  IsDeleted = a.IsDeleted,
                                  IsMultipleBreakdown = a.IsMultipleBreakdown,
                                  PaymentTerms = a.PaymentTerms,
                                  Project = a.Project,
                                  SDate = a.SDate,
                                  SecondVisitDate = a.SecondVisitDate,
                                  ServiceQuote = a.ServiceQuote,
                                  ServiceType = a.ServiceType,
                                  SqDate = a.SqDate,
                                  TnC = a.TnC,
                                  Zerorate = a.Zerorate,
                                  CustSiteName = s.CustRegName,
                                  Period = a.SDate + " - " + a.EDate
                              }).ToListAsync();
            }
            else if (userProfile.ContactType == "CS")
            {
                var sites = userProfile.CustSites.Split(',');
                lstAMCs=  await (from a in context.AMC
                              join s in context.Site on a.CustSite equals s.Id
                              where sites.Contains(s.Id.ToString())
                              select new AmcResponse
                              {
                                  BaseCurrencyAmt = a.BaseCurrencyAmt,
                                  BillTo = a.BillTo,
                                  BrandId = a.BrandId,
                                  ConversionAmount = a.ConversionAmount,
                                  CurrencyId = a.CurrencyId,
                                  CustSite = a.CustSite,
                                  EDate = a.EDate,
                                  FirstVisitDate = a.FirstVisitDate,
                                  Id = a.Id,
                                  IsActive = a.IsActive,
                                  IsDeleted = a.IsDeleted,
                                  IsMultipleBreakdown = a.IsMultipleBreakdown,
                                  PaymentTerms = a.PaymentTerms,
                                  Project = a.Project,
                                  SDate = a.SDate,
                                  SecondVisitDate = a.SecondVisitDate,
                                  ServiceQuote = a.ServiceQuote,
                                  ServiceType = a.ServiceType,
                                  SqDate = a.SqDate,
                                  TnC = a.TnC,
                                  Zerorate = a.Zerorate,
                                  CustSiteName = s.CustRegName,
                                  Period = a.SDate + " - " + a.EDate
                              }).ToListAsync();
            }
            return lstAMCs;
        }

        public async Task<AMC> GetByIdEntityAsync(Guid requestId)
            => await context.AMC.FirstOrDefaultAsync(x => x.Id == requestId);

        public async Task<AMCResponse> GetByIdAsync(Guid requestId)
        {

            var Amc = await context.AMC.FirstOrDefaultAsync(x => x.Id == requestId);
            if (Amc != null)
            {
                var isCompleted = false;
                var stage = "";
                var stageLst = context.AMCStages.Where(x => x.AMCId == Amc.Id && x.IsCompleted == true)?.OrderByDescending(x => x.UpdatedOn).ToList();

                if (stageLst.Count > 0)
                {
                    stage = stageLst[0]?.Stage;
                    stage = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == stage)?.ItemName;
                    stage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stage);

                    var compStage = context.VW_ListItems.FirstOrDefault(x => x.ItemCode == "COMPT" && x.ListCode == "AMCSG");
                    var completedStage = stageLst.FirstOrDefault(x => x.Stage == compStage.ListTypeItemId.ToString());
                    isCompleted = completedStage != null;

                }

                var mAmc = new AMCResponse()
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
                    ConversionAmount = Amc.ConversionAmount,
                    Zerorate = Amc.Zerorate,
                    BaseCurrencyAmt = Amc.BaseCurrencyAmt,
                    TnC = Amc.TnC,
                    StageName = stage,
                    PaymentTerms = Amc.PaymentTerms,
                    SecondVisitDate = Amc.SecondVisitDate,
                    FirstVisitDate = Amc.FirstVisitDate,
                    IsCompleted = isCompleted,
                    IsMultipleBreakdown = Amc.IsMultipleBreakdown,
                    AMCServiceType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == Amc.ServiceType)?.ItemName,
                    AMCServiceTypeCode = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId.ToString() == Amc.ServiceType)?.ItemCode
                };

                return mAmc;
            }
            return null;
        }

        public async Task<Guid> UpdateAmcAsync(AMC updateAmc)
        {
            updateAmc.UpdatedOn = DateTime.Now;
            updateAmc.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(updateAmc).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return updateAmc.Id;
        }

        public async Task<bool> ServiceQuoteExists(string serviceQuote)
        {
            return await context.AMC.AnyAsync(x => x.ServiceQuote == serviceQuote);
        }
    }
}