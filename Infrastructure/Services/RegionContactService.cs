using Application.Features.Distributors;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Application.Features.Distributors.Requests;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class RegionContactService(ApplicationDbContext context, ICurrentUserService currentUserService) : IRegionContactService
    {
        public async Task<List<RegionContact>> GetRegionContactsAsync(Guid regionId)
        => await context.RegionContact.Where(x => x.RegionId == regionId).ToListAsync();

        public Task<RegionContact> GetRegionContactAsync(Guid id)
           => context.RegionContact.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateRegionContactAsync(RegionContact regionContact)
        {
            regionContact.CreatedOn = DateTime.Now;
            regionContact.UpdatedOn = DateTime.Now;
            regionContact.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            regionContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.RegionContact.AddAsync(regionContact);
            await context.SaveChangesAsync();
            return regionContact.Id;
        }

        public async Task<Guid> UpdateRegionContactAsync(RegionContact regionContact)
        {
            regionContact.UpdatedOn = DateTime.Now;
            regionContact.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(regionContact).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return regionContact.Id;
        }

        public async Task<bool> DeleteRegionContactAsync(Guid id)
        {
            var deleteRegionContact = await context
                .RegionContact.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteRegionContact == null) return true;

            deleteRegionContact.IsDeleted = true;
            deleteRegionContact.IsActive = false;

            context.Entry(deleteRegionContact).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RegionContact>> GetDistributorRegionEngineers(Guid distId, string code)
        {
            if (!string.IsNullOrEmpty(code) && code.ToUpper() == "ENGINEER")
            {
                return await (from r in context.Regions.Where(x => x.DistId == distId)
                              join rc in context.RegionContact on r.Id equals rc.RegionId
                              where rc.IsFieldEngineer == true
                              select rc).ToListAsync();
            }
            else
                return await (from r in context.Regions.Where(x => x.DistId == distId)
                              join rc in context.RegionContact on r.Id equals rc.RegionId
                              select rc).ToListAsync();
        }

        public async Task<List<RegionContact>> GetRegionContactByContact(Guid contactId)
        {
            var userProfile = context.VW_UserProfile.FirstOrDefault(x => x.ContactId == contactId);
            return await (from r in context.Regions.Where(x => x.DistId == userProfile.EntityParentId)
                          join rc in context.RegionContact on r.Id equals rc.RegionId
                          select rc).ToListAsync();
        }

        public async Task<bool> IsDuplicateAsync(RegionContactRequest regionContact)
        {
            var isValid = false;
            if (await context.RegionContact.AnyAsync(x => x.PrimaryEmail == regionContact.PrimaryEmail))
            {
                isValid = true;
            }
            else if (await context.RegionContact.AnyAsync(x => x.FirstName == regionContact.FirstName && x.LastName == regionContact.LastName))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}