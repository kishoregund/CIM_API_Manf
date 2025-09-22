using Application.Features.Distributors;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System.Diagnostics.Metrics;
using Domain.Views;
using Application.Features.Distributors.Responses;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

namespace Infrastructure.Services
{
    public class DistributorService(ApplicationDbContext context, ICurrentUserService currentUserService) : IDistributorService
    {
        public async Task<List<DistributorResponse>> GetDistributorsAsync()
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());

            var distributors = await (from d in context.Distributor
                                      join pt in context.ListTypeItems on d.Payterms equals pt.Id.ToString()
                                      join c in context.Country on d.AddrCountryId equals c.Id
                                      //where d.Id == userProfile.EntityParentId
                                      select new DistributorResponse()
                                      {
                                          AddrCountryId = d.AddrCountryId.ToString(),
                                          AddrCountryName = c.Name,
                                          Area = d.Area,
                                          City = d.City,
                                          Code = d.Code,
                                          DistName = d.DistName,
                                          GeoLat = d.GeoLat.ToString(),
                                          GeoLong = d.GeoLong.ToString(),
                                          Id = d.Id,
                                          IsBlocked = d.IsBlocked,
                                          ManufacturerIds = d.ManufacturerIds,
                                          Payterms = d.Payterms,
                                          PaytermsName = pt.ItemName,
                                          Place = d.Place,
                                          Street = d.Street,
                                          Zip = d.Zip,
                                      }).ToListAsync();

            if (userProfile != null && userProfile.ContactType.ToUpper() == "DR")
                distributors = distributors.Where(x => x.Id == userProfile.EntityParentId).ToList();

            return distributors;
        }

        public async Task<DistributorResponse> GetDistributorAsync(Guid id)
        {
            var distributors = await this.GetDistributorsAsync();
            return distributors.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<Distributor> GetDistributorEntityAsync(Guid id)
            => await context.Distributor.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateDistributorAsync(Distributor distributor)
        {
            var country = context.Country.Find(distributor.AddrCountryId);
            distributor.Code = $"D{(distributor.DistName.Length >= 3 ? distributor.DistName.Substring(0, 3).ToUpper() : distributor.DistName.Substring(0, 1).ToUpper())}{country?.Iso_2}{DateTime.Now.ToString("yy") + DateTime.Now.DayOfYear + DateTime.Now.ToString("HHmm")}";
            distributor.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            distributor.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            distributor.CreatedOn = DateTime.Now;
            distributor.UpdatedOn = DateTime.Now;
            distributor.IsActive = true;
            distributor.IsDeleted = false;

            await context.Distributor.AddAsync(distributor);
            await context.SaveChangesAsync();
            return distributor.Id;
        }

        public async Task<Guid> UpdateDistributorAsync(Distributor distributor)
        {
            distributor.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            distributor.UpdatedOn = DateTime.Now;

            context.Entry(distributor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return distributor.Id;
        }

        public async Task<bool> DeleteDistributorAsync(Guid id)
        {
            var deleteDistributor = await context.Distributor.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteDistributor == null) return true;

            deleteDistributor.IsDeleted = true;
            deleteDistributor.IsActive = false;

            context.Entry(deleteDistributor).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DistributorResponse>> GetDistributorsByContactAsync(Guid contactId)
        {
            var userProfile = context.VW_UserProfile.FirstOrDefault(x => x.ContactId == contactId);
            List<Distributor> lstDistrbutors = new();
            if (userProfile != null && userProfile.ContactType == "DR")
            {
                lstDistrbutors = await context.Distributor.Where(x => x.Id == userProfile.EntityParentId).ToListAsync();

            }
            else if (userProfile != null && userProfile.ContactType == "CS")
            {
                lstDistrbutors = await (from d in context.Distributor
                                        join c in context.Customer on d.Id equals c.DefDistId
                                        where c.Id == userProfile.EntityParentId
                                        select d).ToListAsync();
            }
            else
            {
                lstDistrbutors = await context.Distributor.ToListAsync();
            }
            var distributors = new List<DistributorResponse>();
            if (lstDistrbutors.Count > 0)
            {
                distributors = (from d in lstDistrbutors
                                join pt in context.ListTypeItems on d.Payterms equals pt.Id.ToString()
                                select new DistributorResponse()
                                {
                                    AddrCountryId = d.AddrCountryId.ToString(),
                                    Area = d.Area,
                                    City = d.City,
                                    Code = d.Code,
                                    DistName = d.DistName,
                                    GeoLat = d.GeoLat.ToString(),
                                    GeoLong = d.GeoLong.ToString(),
                                    Id = d.Id,
                                    IsBlocked = d.IsBlocked,
                                    ManufacturerIds = d.ManufacturerIds,
                                    Payterms = d.Payterms,
                                    PaytermsName = pt.ItemName,
                                    Place = d.Place,
                                    Street = d.Street,
                                    Zip = d.Zip,
                                }).ToList();
            }
            return distributors;
        }

        public async Task<bool> IsDuplicateAsync(string distributorName)
           => await context.Distributor.AnyAsync(x => x.DistName.ToUpper() == distributorName.ToUpper());

    }
}
