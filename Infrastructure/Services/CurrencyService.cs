using Application.Features.Identity.Users;
using Application.Features.Masters;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Infrastructure.Services
{
    public class CurrencyService(ApplicationDbContext Context, ICurrentUserService currentUserService) : ICurrencyService
    {

        public async Task<Currency> GetCurrencyAsync(Guid id)
            => await Context.Currency.FirstOrDefaultAsync(p => p.Id == id);


        public async Task<List<Currency>> GetCurrenciesAsync()
            => await Context.Currency.OrderBy(x=>x.Code).ToListAsync();

        public async Task<Guid> CreateCurrencyAsync(Currency currency)
        {
            currency.CreatedOn = DateTime.Now;
            currency.UpdatedOn = DateTime.Now;
            currency.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            currency.CreatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.Currency.AddAsync(currency);
            await Context.SaveChangesAsync();
            return currency.Id;
        }

        public async Task<bool> DeleteCurrencyAsync(Guid id)
        {

            var deletedCurrency = await Context
                .Currency.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deletedCurrency != null)
            {
                //deletedCurrency.IsDeleted = true;
                //deletedCurrency.IsActive = false;

                Context.Entry(deletedCurrency).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Guid> UpdateCurrencyAsync(Currency currency)
        {
            currency.UpdatedOn = DateTime.Now;
            currency.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(currency).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return currency.Id;
        }

    }
}