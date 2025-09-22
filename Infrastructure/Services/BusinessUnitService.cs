using Application.Features.AppBasic;
using Application.Features.Identity.Users;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class BusinessUnitService(ApplicationDbContext context, ICurrentUserService currentUserService) : IBusinessUnitService
    {
        public async Task<List<BusinessUnit>> GetBusinessUnitsAsync()
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            if (userProfile != null && userProfile.ContactType == "DR")
            {
                var bus = userProfile.BusinessUnitIds.Split(',');
                return await context.BusinessUnit.Where(x => bus.Contains(x.Id.ToString())).ToListAsync();
            }
            return await context.BusinessUnit.ToListAsync();
        }

        public async Task<BusinessUnit> GetBusinessUnitByIdAsync(Guid id)
            => await context.BusinessUnit.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateBusinessUnitAsync(BusinessUnit BusinessUnit)
        {
            BusinessUnit.CreatedOn = DateTime.Now;
            BusinessUnit.UpdatedOn = DateTime.Now;
            BusinessUnit.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            BusinessUnit.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.BusinessUnit.AddAsync(BusinessUnit);
            await context.SaveChangesAsync();
            return BusinessUnit.Id;
        }

        public async Task<Guid> UpdateBusinessUnitAsync(BusinessUnit BusinessUnit)
        {
            BusinessUnit.UpdatedOn = DateTime.Now;
            BusinessUnit.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(BusinessUnit).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return BusinessUnit.Id;
        }

        public async Task<bool> DeleteBusinessUnitAsync(Guid id)
        {

            var deleteBusinessUnit = await context
                .BusinessUnit.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteBusinessUnit == null) return true;
                        
            context.Entry(deleteBusinessUnit).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string buName)
        {
            return await context.BusinessUnit.AnyAsync(x => x.BusinessUnitName.ToUpper() == buName.ToUpper());
        }
        

    }
}
