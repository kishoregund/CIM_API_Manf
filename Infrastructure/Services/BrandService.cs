using Application.Features.AppBasic;
using Application.Features.AppBasic.Responses;
using Application.Features.Identity.Users;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Services
{
    public class BrandService(ApplicationDbContext context, ICurrentUserService currentUserService) : IBrandService
    {
        public async Task<List<BrandResponse>> GetBrandsByBusinessUnitAsync(Guid businessUnitId)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

            var brands = await (from b in context.Brand
                                join bu in context.BusinessUnit on b.BusinessUnitId equals bu.Id
                                select new BrandResponse
                                {
                                    BusinessUnitName = bu.BusinessUnitName,
                                    BusinessUnitId = b.BusinessUnitId,
                                    BrandName = b.BrandName,
                                    CreatedBy = b.CreatedBy,
                                    CreatedOn = b.CreatedOn,
                                    Id = b.Id,
                                    IsActive = b.IsActive,
                                    IsDeleted = b.IsDeleted,
                                    UpdatedBy = b.UpdatedBy,
                                    UpdatedOn = b.UpdatedOn
                                }).ToListAsync();

            if (userProfile != null && userProfile.ContactType == "DR" && userProfile.SegmentCode == "RDTSP")
                brands.Where(x => x.BusinessUnitId == businessUnitId).ToList();

            return brands;
        }

        public async Task<List<BrandResponse>> GetBrandsByBusinessUnitsAsync(string businessUnits)
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            List<string> lstBusinessUnit = businessUnits.Split(",").ToList();
            var brands = await (from b in context.Brand
                                join bu in context.BusinessUnit on b.BusinessUnitId equals bu.Id
                                select new BrandResponse
                                {
                                    BusinessUnitName = bu.BusinessUnitName,
                                    BusinessUnitId = b.BusinessUnitId,
                                    BrandName = b.BrandName,
                                    CreatedBy = b.CreatedBy,
                                    CreatedOn = b.CreatedOn,
                                    Id = b.Id,
                                    IsActive = b.IsActive,
                                    IsDeleted = b.IsDeleted,
                                    UpdatedBy = b.UpdatedBy,
                                    UpdatedOn = b.UpdatedOn
                                }).ToListAsync();

            if (userProfile != null && userProfile.ContactType == "DR" && userProfile.SegmentCode == "RDTSP")
                brands.Where(x => lstBusinessUnit.Contains(x.BusinessUnitId.ToString())).ToList();

            return brands;
        }

        public async Task<List<BrandResponse>> GetBrandsAsync()
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            var brands = await context.Brand.ToListAsync();
            if (userProfile != null && userProfile.ContactType == "DR")
            {
                var bIds = userProfile.BrandIds.Split(',');
                brands = brands.Where(x => bIds.Contains(x.Id.ToString())).ToList();
            }

            return (from b in brands
                    join bu in context.BusinessUnit on b.BusinessUnitId equals bu.Id
                    select new BrandResponse
                    {
                        BusinessUnitName = bu.BusinessUnitName,
                        BusinessUnitId = b.BusinessUnitId,
                        BrandName = b.BrandName,
                        CreatedBy = b.CreatedBy,
                        CreatedOn = b.CreatedOn,
                        Id = b.Id,
                        IsActive = b.IsActive,
                        IsDeleted = b.IsDeleted,
                        UpdatedBy = b.UpdatedBy,
                        UpdatedOn = b.UpdatedOn
                    }).ToList();
        }

        public async Task<Brand> GetBrandByIdAsync(Guid id)
            => await context.Brand.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateBrandAsync(Brand Brand)
        {
            Brand.CreatedOn = DateTime.Now;
            Brand.UpdatedOn = DateTime.Now;
            Brand.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Brand.CreatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.Brand.AddAsync(Brand);
            await context.SaveChangesAsync();
            return Brand.Id;
        }

        public async Task<Guid> UpdateBrandAsync(Brand Brand)
        {
            Brand.UpdatedOn = DateTime.Now;
            Brand.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(Brand).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Brand.Id;
        }

        public async Task<bool> DeleteBrandAsync(Guid id)
        {

            var deleteBrand = await context
                .Brand.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteBrand == null) return true;

            context.Entry(deleteBrand).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string brandName, Guid businessUnitId)
            => await context.Brand.AnyAsync(x => x.BusinessUnitId == businessUnitId && x.BrandName.ToUpper() == brandName.ToUpper());

    }
}
