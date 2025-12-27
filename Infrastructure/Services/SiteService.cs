
using Application.Features.Customers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using Application.Features.Customers.Responses;
using Mapster;
using Application.Features.UserProfiles.Responses;


namespace Infrastructure.Services
{
    public class SiteService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ISiteService
    {
        public async Task<Site> GetSiteAsync(Guid id)
            => await Context.Site.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Site>> GetSitesAsync(Guid customerId)
            => await Context.Site.Where(p => p.CustomerId == customerId).ToListAsync();

        public async Task<List<Site>> GetSitesbyUserIdAsync(Guid customerId)
        {            
            var userProfile = Context.VW_UserProfile.FirstOrDefault(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            if (userProfile == null)
                return null;

            if (userProfile.ContactType == "DR")
            {
                var regions = userProfile.DistRegions.Split(',');
                return await (from c in Context.Customer.Where(x => x.DefDistId == userProfile.EntityParentId)
                              join s in Context.Site on c.Id equals s.CustomerId
                              where regions.Contains(s.RegionId.ToString()) && s.CustomerId == customerId
                              select s).Distinct().ToListAsync();
            }
            else
            {
                var sites = userProfile.CustSites.Split(',');
                return await (from s in Context.Site.Where(x => x.CustomerId == customerId)
                              where sites.Contains(s.Id.ToString())
                              select s).Distinct().ToListAsync();
            }
        }

        public async Task<List<SiteResponse>> GetSitesByContactAsync(Guid contactId)
        {
            return (await (from s in Context.Site
                           join sc in Context.SiteContact on s.Id equals sc.SiteId
                           where sc.Id == contactId
                           select s).ToListAsync()).Adapt<List<SiteResponse>>();
        }

        public async Task<List<UserByContactResponse>> GetSiteUsersAsync(Guid siteId)
        { 
            var users = await (from u in Context.VW_UserProfile.Where(x => x.CustSites.Contains(siteId.ToString()))
                         select new UserByContactResponse()
                         {
                             ContactType = u.ContactType,
                             Email = u.Email,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             UserId = u.UserId.ToString().ToLower()
                         }).ToListAsync();

            return users;
        }

        public async Task<Guid> CreateSiteAsync(Site Site)
        {
            Site.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            Site.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Site.CreatedOn = DateTime.Now;
            Site.UpdatedOn = DateTime.Now;
            

            await Context.Site.AddAsync(Site);
            await Context.SaveChangesAsync();

            return Site.Id;
        }

        public async Task<bool> DeleteSiteAsync(Guid id)
        {
            var deletedSite = await Context
                .Site.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedSite != null)
            {
                deletedSite.IsDeleted = true;
                deletedSite.IsActive = false;

                Context.Entry(deletedSite).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Guid> UpdateSiteAsync(Site Site)
        {
            Site.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Site.UpdatedOn = DateTime.Now;

            Context.Entry(Site).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return Site.Id;
        }

        public async Task<List<Regions>> GetDistRegionsByCustomerAsync(Guid custId)
        {
            var defDistId = Context.Customer.FirstOrDefault(x => x.Id == custId)?.DefDistId;

            return await Context.Regions.Where(x => x.DistId == defDistId).OrderBy(x => x.DistRegName).ToListAsync();
        }

        public async Task<bool> IsDuplicateAsync(string custRegName, Guid custId)
            => await Context.Site.AnyAsync(x => x.CustRegName.ToUpper() == custRegName.ToUpper() && x.CustomerId == custId);

    }
}

