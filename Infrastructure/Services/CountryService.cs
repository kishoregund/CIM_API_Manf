using Application.Features.Identity.Users;
using Application.Features.Masters;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CountryService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ICountryService
    {

        public async Task<Country> GetCountryAsync(Guid id)
            => await Context.Country.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Country>> GetCountriesAsync()
            => await Context.Country.OrderBy(x=>x.Iso_3).ToListAsync();

        public async Task<Guid> CreateCountryAsync(Country country)
        {
            country.CreatedOn = DateTime.Now;
            country.UpdatedOn = DateTime.Now;
            country.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            country.CreatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.Country.AddAsync(country);
            await Context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<bool> DeleteCountryAsync(Guid id)
        {

            var deletedCountry = await Context
                .Country.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedCountry == null) return true;

            //deletedCountry.IsDeleted = true;
            //deletedCountry.IsActive = false;

            Context.Entry(deletedCountry).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<Guid> UpdateCountryAsync(Country country)
        {
            country.UpdatedOn = DateTime.Now;
            country.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(country).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return country.Id;
        }

    }
}