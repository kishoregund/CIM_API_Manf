using Application.Features.AppBasic;
using Application.Features.Identity.Users;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ManfBusinessUnitService(ApplicationDbContext context, ICurrentUserService currentUserService) : IManfBusinessUnitService
    {
        public async Task<List<ManfBusinessUnit>> GetManfBusinessUnitsAsync()
        {
            //var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            //if (userProfile != null && userProfile.ContactType == "DR")
            //{
            //    var bus = userProfile.ManfBusinessUnitIds.Split(',');
            //    return await context.ManfBusinessUnit.Where(x => bus.Contains(x.Id.ToString())).ToListAsync();
            //}
            return await context.ManfBusinessUnit.ToListAsync();
        }

        public async Task<ManfBusinessUnit> GetManfBusinessUnitByIdAsync(Guid id)
            => await context.ManfBusinessUnit.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateManfBusinessUnitAsync(ManfBusinessUnit ManfBusinessUnit)
        {
            ManfBusinessUnit.CreatedOn = DateTime.Now;
            ManfBusinessUnit.UpdatedOn = DateTime.Now;
            ManfBusinessUnit.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            ManfBusinessUnit.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.ManfBusinessUnit.AddAsync(ManfBusinessUnit);
            await context.SaveChangesAsync();
            return ManfBusinessUnit.Id;
        }

        public async Task<Guid> UpdateManfBusinessUnitAsync(ManfBusinessUnit ManfBusinessUnit)
        {
            ManfBusinessUnit.UpdatedOn = DateTime.Now;
            ManfBusinessUnit.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(ManfBusinessUnit).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return ManfBusinessUnit.Id;
        }

        public async Task<bool> DeleteManfBusinessUnitAsync(Guid id)
        {

            var deleteManfBusinessUnit = await context
                .ManfBusinessUnit.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteManfBusinessUnit == null) return true;
                        
            context.Entry(deleteManfBusinessUnit).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string buName)
        {
            return await context.ManfBusinessUnit.AnyAsync(x => x.BusinessUnitName.ToUpper() == buName.ToUpper());
        }
        

    }
}
