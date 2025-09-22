
using Application.Features.Customers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Domain.Views;
using Application.Features.Distributors.Requests;
using Application.Features.Customers.Requests;

namespace Infrastructure.Services
{
    public class SiteContactService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ISiteContactService
    {

        public async Task<SiteContact> GetSiteContactAsync(Guid id)
          => await Context.SiteContact.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<SiteContact>> GetSiteContactsAsync(Guid siteId)
          => await Context.SiteContact.Where(p => p.SiteId == siteId).ToListAsync();

        public async Task<Guid> CreateSiteContactAsync(SiteContact SiteContact)
        {
            SiteContact.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            SiteContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SiteContact.CreatedOn = DateTime.Now;
            SiteContact.UpdatedOn = DateTime.Now;

            await Context.SiteContact.AddAsync(SiteContact);
            await Context.SaveChangesAsync();

            return SiteContact.Id;
        }

        public async Task<bool> DeleteSiteContactAsync(Guid id)
        {

            var deletedSiteContact = await Context
                .SiteContact.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedSiteContact != null)
            {
                deletedSiteContact.IsDeleted = true;
                deletedSiteContact.IsActive = false;

                Context.Entry(deletedSiteContact).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Guid> UpdateSiteContactAsync(SiteContact SiteContact)
        {
            SiteContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            SiteContact.UpdatedOn = DateTime.Now;

            Context.Entry(SiteContact).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return SiteContact.Id;
        }

        public async Task<List<SiteContact>> GetSiteContactsByUserIdAsync()
        {
            VW_UserProfile prof = await Context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));            
            if (prof.ContactType == "CS" || prof.ContactType == "C")
            {
                return  await (from sc in Context.SiteContact
                                join s in Context.Site on sc.SiteId equals s.Id
                                join c in Context.Customer on s.CustomerId equals c.Id
                                where c.Id == prof.EntityParentId
                                select sc).ToListAsync();

            }
            return null;
        }

        public async Task<List<SiteContact>> GetSiteContactsByCustomerAsync(Guid customerId)
        {
            var contacts = await (from c in Context.Customer
                            join s in Context.Site on c.Id equals s.CustomerId
                            join sc in Context.SiteContact on s.Id equals sc.SiteId
                            where c.Id == customerId
                            select sc).ToListAsync();

            return contacts;
        }

        public async Task<bool> IsDuplicateAsync(SiteContactRequest siteContact)
        {
            var isValid = false;
            if (await Context.SiteContact.AnyAsync(x => x.PrimaryEmail == siteContact.PrimaryEmail))
            {
                isValid = true;
            }
            else if (await Context.SiteContact.AnyAsync(x => x.FirstName == siteContact.FirstName && x.LastName == siteContact.LastName))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}

