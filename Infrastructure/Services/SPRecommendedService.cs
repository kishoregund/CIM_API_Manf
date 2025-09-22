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
            return SPRecommended.Id;
        }

    }
}
