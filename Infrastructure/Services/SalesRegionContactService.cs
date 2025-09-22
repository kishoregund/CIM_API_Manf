using Application.Features.Manufacturers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Application.Features.Distributors.Requests;
using Application.Features.Manufacturers.Requests;

namespace Infrastructure.Services
{
    public class SalesRegionContactService(ApplicationDbContext context, ICurrentUserService currentUserService) : ISalesRegionContactService
    {
        public async Task<List<SalesRegionContact>> GetSalesRegionContactsAsync(Guid SalesRegionId)
            => await context.SalesRegionContact.Where(x=>x.SalesRegionId == SalesRegionId).ToListAsync();

        public async Task<SalesRegionContact> GetSalesRegionContactAsync(Guid id)
            => await context.SalesRegionContact.FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task<Guid> CreateSalesRegionContactAsync(SalesRegionContact SalesRegionContact)
        {
            SalesRegionContact.CreatedOn = DateTime.Now;
            SalesRegionContact.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SalesRegionContact.UpdatedOn = DateTime.Now;
            SalesRegionContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SalesRegionContact.IsActive = true;
            SalesRegionContact.IsDeleted = true;

            await context.SalesRegionContact.AddAsync(SalesRegionContact);
            await context.SaveChangesAsync();
            return SalesRegionContact.Id;
        }

        public async Task<Guid> UpdateSalesRegionContactAsync(SalesRegionContact SalesRegionContact)
        {
            SalesRegionContact.UpdatedOn = DateTime.Now;
            SalesRegionContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(SalesRegionContact).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return SalesRegionContact.Id;
        }

        public async Task<bool> DeleteSalesRegionContactAsync(Guid id)
        {
            var deleteSalesRegionContact = await context.SalesRegionContact.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteSalesRegionContact == null) return true;

            deleteSalesRegionContact.IsDeleted = true;
            deleteSalesRegionContact.IsActive = false;

            context.Entry(deleteSalesRegionContact).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(SalesRegionContactRequest regionContact)
        {
            var isValid = false;
            if (await context.SalesRegionContact.AnyAsync(x => x.PrimaryEmail == regionContact.PrimaryEmail))
            {
                isValid = true;
            }
            else if (await context.SalesRegionContact.AnyAsync(x => x.FirstName == regionContact.FirstName && x.LastName == regionContact.LastName))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}