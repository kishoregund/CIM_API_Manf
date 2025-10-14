//using Dapper;
using Application.Features.Identity.Users;
using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Responses;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ManufacturerService(ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration) : IManufacturerService
    {
        public async Task<List<ManufacturerResponse>> GetManufacturersAsync()
        {
            var manufacturers = await (from d in context.Manufacturer
                                      join pt in context.ListTypeItems on d.Payterms equals pt.Id.ToString()
                                      select new ManufacturerResponse()
                                      {
                                          AddrCountryId = d.AddrCountryId.ToString(),
                                          Area = d.Area,
                                          City = d.City,
                                          Code = d.Code,
                                          ManfName = d.ManfName,
                                          GeoLat = d.GeoLat.ToString(),
                                          GeoLong = d.GeoLong.ToString(),
                                          Id = d.Id,
                                          IsBlocked = d.IsBlocked,
                                          Payterms = d.Payterms,
                                          PaytermsName = pt.ItemName,
                                          Place = d.Place,
                                          Street = d.Street,
                                          Zip = d.Zip,
                                      }).ToListAsync();

            return manufacturers;
        }
        public async Task<Domain.Entities.Manufacturer> GetManufacturerAsync(Guid id)
            => await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task<Guid> CreateManufacturerAsync(Domain.Entities.Manufacturer Manufacturer)
        {
            var country = context.Country.Find(Manufacturer.AddrCountryId);
            Manufacturer.Code = $"M{(Manufacturer.ManfName.Length >= 3 ? Manufacturer.ManfName.Substring(0, 3).ToUpper() : Manufacturer.ManfName.Substring(0, 1).ToUpper())}{country?.Iso_2}{DateTime.Now.ToString("yy") + DateTime.Now.DayOfYear + DateTime.Now.ToString("HHmm")}";
            Manufacturer.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            Manufacturer.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Manufacturer.CreatedOn = DateTime.Now;
            Manufacturer.UpdatedOn = DateTime.Now;
            Manufacturer.IsActive = true;
            Manufacturer.IsDeleted = false;
            await context.Manufacturer.AddAsync(Manufacturer);
            await context.SaveChangesAsync();
            return Manufacturer.Id;
        }
        public async Task<Guid> UpdateManufacturerAsync(Domain.Entities.Manufacturer Manufacturer)
        {
            Manufacturer.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            Manufacturer.UpdatedOn = DateTime.Now;

            context.Entry(Manufacturer).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Manufacturer.Id;
        }

        public async Task<bool> DeleteManufacturerAsync(Guid id)
        {
            var deleteManufacturer = await context.Manufacturer.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteManufacturer == null) return true;

            deleteManufacturer.IsDeleted = true;
            deleteManufacturer.IsActive = false;

            context.Entry(deleteManufacturer).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(string manufacturerName)
            => await context.Manufacturer.AnyAsync(x => x.ManfName.ToUpper() == manufacturerName.ToUpper());

        public async Task<bool> IsManfSubscribedAsync()
        {
            CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);
            if (commonMethods.IsManfSubscribed() && await context.Manufacturer.CountAsync() > 0)
                return false;
            else return true;
        }
    }
}