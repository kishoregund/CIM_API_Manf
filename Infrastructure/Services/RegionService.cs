using Application.Features.Distributors;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class RegionService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : IRegionService
    {
        public async Task<List<Regions>> GetRegionsByDistributorAsync(Guid distributorId)
            => await context.Regions.Where(x => x.DistId == distributorId).ToListAsync();

        public async Task<List<Regions>> GetRegionsAsync()
            => await context.Regions.ToListAsync();

        public async Task<List<Regions>> GetAssignedRegionsAsync()
        {
            CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);
            var regions = await commonMethods.GetDistRegionsByUserIdAsync();

            return await context.Regions.Where(x => regions.Contains(x.Id.ToString())).ToListAsync();
        }


        public async Task<Regions> GetRegionAsync(Guid id)
            => await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateRegionAsync(Regions region)
        {
            region.CreatedOn = DateTime.Now;
            region.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            region.UpdatedOn = DateTime.Now;
            region.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.Regions.AddAsync(region);
            await context.SaveChangesAsync();
            return region.Id;
        }

        public async Task<Guid> UpdateRegionAsync(Regions region)
        {
            region.UpdatedOn = DateTime.Now;
            region.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(region).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return region.Id;
        }

        public async Task<bool> DeleteRegionAsync(Guid id)
        {

            var deleteRegion = await context
                .Regions.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteRegion == null) return true;

            deleteRegion.IsDeleted = true;
            deleteRegion.IsActive = false;

            context.Entry(deleteRegion).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string distRegName, Guid distId)
         => await context.Regions.AnyAsync(x => x.DistRegName.ToUpper() == distRegName.ToUpper() && x.DistId == distId);


    }
}
