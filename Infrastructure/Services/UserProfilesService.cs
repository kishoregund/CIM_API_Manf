using Application.Features.Identity.Users;
using Application.Features.UserProfiles;
using Application.Features.UserProfiles.Responses;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{

    public class UserProfilesService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IUserProfilesService
    {

        
        public async Task<UserProfiles> GetUserProfileEntityAsync(Guid id)
            => await Context.UserProfiles.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<VW_UserProfile> GetUserProfileAsync(Guid id)
            => await Context.VW_UserProfile.FirstOrDefaultAsync(p => p.UserProfileId == id);

        public async Task<List<VW_UserProfile>> GetUserProfilesAsync()
        {
            var listUserProfiles = await Context.VW_UserProfile.ToListAsync();

            //var userProfiles = await Context.UserProfiles.ToListAsync();
            //UserProfilesListResponse response;
            //List<UserProfilesListResponse> listUserProfiles = new List<UserProfilesListResponse>();
            //foreach (var profile in userProfiles)
            //{
            //    response = new UserProfilesListResponse();
            //    var usr = Context.Users.FirstOrDefaultAsync(x => x.Id == profile.UserId.ToString());
            //    response.UserName = usr.Result.FirstName + " " + usr.Result.LastName;
            //    response.RoleName = Context.Roles.FirstOrDefaultAsync(x => x.Id == profile.RoleId.ToString()).Result.Name;
            //    response.Description = profile.Description;
            //    response.Id = profile.Id;

            //    listUserProfiles.Add(response);
            //}

            return listUserProfiles;
        }

        public async Task<Guid> CreateUserProfilesAsync(UserProfiles UserProfiles)
        {
            UserProfiles.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            UserProfiles.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            UserProfiles.UpdatedOn = DateTime.Now;
            UserProfiles.CreatedOn = DateTime.Now;

            await Context.UserProfiles.AddAsync(UserProfiles);
            await Context.SaveChangesAsync();
            return UserProfiles.Id;
        }
        public async Task<bool> DeleteUserProfilesAsync(Guid id)
        {

            var deletedEngAction = await Context
                .UserProfiles.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedEngAction == null) return true;

            deletedEngAction.IsDeleted = true;
            deletedEngAction.IsActive = false;

            Context.Entry(deletedEngAction).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }
        public async Task<Guid> UpdateUserProfilesAsync(UserProfiles UserProfiles)
        {
            UserProfiles.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            UserProfiles.UpdatedOn = DateTime.Now;

            Context.Entry(UserProfiles).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return UserProfiles.Id;
        }
        public async Task<List<Regions>> GetRegionsByContactIdAsync(Guid contactId)
        {
            var regions = await (from d in Context.Distributor
                                 join r in Context.Regions on d.Id equals r.DistId
                                 where d.Id == (
                                     from rc in Context.RegionContact
                                     join r1 in Context.Regions on rc.RegionId equals r1.Id
                                     join d1 in Context.Distributor on r1.DistId equals d1.Id
                                     where rc.Id == contactId
                                     select d1.Id
                                     ).FirstOrDefault()
                                 select r).ToListAsync();

            return regions;
        }
        public async Task<List<Site>> GetSitesByContactIdAsync(Guid contactId)
        {
            var sites = await (from c in Context.Customer
                               join s in Context.Site on c.Id equals s.CustomerId
                               where c.Id == (
                                   from sc in Context.SiteContact
                                   join s1 in Context.Site on sc.SiteId equals s1.Id
                                   join c1 in Context.Customer on s1.CustomerId equals c1.Id
                                   where sc.Id == contactId
                                   select c1.Id
                                   ).FirstOrDefault()
                               select s).ToListAsync();

            return sites;
        }

        public async Task<UserByContactResponse> GetDistUserByContactAsync(Guid contactId)
        {
            var regions = await (from rc in Context.RegionContact
                                 join r1 in Context.Regions on rc.RegionId equals r1.Id
                                 join d1 in Context.Distributor on r1.DistId equals d1.Id
                                 join des in Context.VW_ListItems on rc.DesignationId equals des.ListTypeItemId
                                 where rc.Id == contactId
                                 select new UserByContactResponse()
                                 {
                                     ContactId = rc.Id,
                                     ChildId = r1.Id,
                                     ChildName = r1.DistRegName,
                                     ContactType = "DR",
                                     Designation = des.ItemName,
                                     DesignationId = des.ListTypeItemId,
                                     Email = rc.PrimaryEmail,
                                     FirstName = rc.FirstName,
                                     IsActive = rc.IsActive,
                                     LastName = rc.LastName,
                                     ParentId = d1.Id,
                                     ParentName = d1.DistName,
                                     PhoneNumber = rc.PrimaryContactNo
                                 }).FirstOrDefaultAsync();

            return regions;
        }
        public async Task<UserByContactResponse> GetCustUserByContactAsync(Guid contactId)
        {
            var regions = await (from sc in Context.SiteContact
                                 join s1 in Context.Site on sc.SiteId equals s1.Id
                                 join c1 in Context.Customer on s1.CustomerId equals c1.Id
                                 join des in Context.VW_ListItems on sc.DesignationId equals des.ListTypeItemId
                                 where sc.Id == contactId
                                 select new UserByContactResponse()
                                 {
                                     ContactId = sc.Id,
                                     ChildId = s1.Id,
                                     ChildName = s1.CustRegName,
                                     ContactType = "CS",
                                     Designation = des.ItemName,
                                     DesignationId = des.ListTypeItemId,
                                     Email = sc.PrimaryEmail,
                                     FirstName = sc.FirstName,
                                     IsActive = sc.IsActive,
                                     LastName = sc.LastName,
                                     ParentId = s1.Id,
                                     ParentName = s1.CustRegName,
                                     PhoneNumber = sc.PrimaryContactNo
                                 }).FirstOrDefaultAsync();

            return regions;
        }
        public async Task<UserByContactResponse> GetManfUserByContactAsync(Guid contactId)
        {
            var regions = await (from rc in Context.SalesRegionContact
                                 join r1 in Context.SalesRegion on rc.SalesRegionId equals r1.Id
                                 join d1 in Context.Manufacturer on r1.ManfId equals d1.Id
                                 join des in Context.VW_ListItems on rc.DesignationId equals des.ListTypeItemId
                                 where rc.Id == contactId
                                 select new UserByContactResponse()
                                 {
                                     ContactId = rc.Id,
                                     ChildId = r1.Id,
                                     ChildName = r1.SalesRegionName,
                                     ContactType = "MSR",
                                     Designation = des.ItemName,
                                     DesignationId = des.ListTypeItemId,
                                     Email = rc.PrimaryEmail,
                                     FirstName = rc.FirstName,
                                     IsActive = rc.IsActive,
                                     LastName = rc.LastName,
                                     ParentId = d1.Id,
                                     ParentName = d1.ManfName,
                                     PhoneNumber = rc.PrimaryContactNo
                                 }).FirstOrDefaultAsync();

            return regions;
        }
    }
}