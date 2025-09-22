using Application.Features.Manufacturers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;

namespace Infrastructure.Services
{
    public class SalesRegionService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISalesRegionService
    {
        public async Task<List<SalesRegion>> GetSalesRegionsAsync(Guid ManufacturerId)
            => await context.SalesRegion.Where(x => x.ManfId == ManufacturerId).ToListAsync();

        public async Task<SalesRegion> GetSalesRegionAsync(Guid id)
            => await context.SalesRegion.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateSalesRegionAsync(SalesRegion SalesRegion)
        {
            SalesRegion.CreatedOn = DateTime.Now;
            SalesRegion.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SalesRegion.UpdatedOn = DateTime.Now;
            SalesRegion.UpdatedBy = Guid.Parse(currentUserService.GetUserId());


            await context.SalesRegion.AddAsync(SalesRegion);
            await context.SaveChangesAsync();
            return SalesRegion.Id;
        }

        public async Task<Guid> UpdateSalesRegionAsync(SalesRegion SalesRegion)
        {
            SalesRegion.UpdatedOn = DateTime.Now;
            SalesRegion.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(SalesRegion).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SalesRegion.Id;
        }

        public async Task<bool> DeleteSalesRegionAsync(Guid id)
        {
            var deleteSalesRegion = await context
                .SalesRegion.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteSalesRegion == null) return true;

            deleteSalesRegion.IsDeleted = true;
            deleteSalesRegion.IsActive = false;

            context.Entry(deleteSalesRegion).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string salesRegName, Guid manfId)
            => await context.SalesRegion.AnyAsync(x => x.SalesRegionName.ToUpper() == salesRegName.ToUpper() && x.ManfId == manfId);

    }
}