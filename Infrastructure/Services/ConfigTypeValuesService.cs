using Application.Features.Identity.Users;
using Application.Features.Masters;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services
{
    public class ConfigTypeValuesService(ApplicationDbContext Context,ICurrentUserService currentUserService) : IConfigTypeValuesService

    { 
       public async Task<ConfigTypeValues> GetConfigTypeValueAsync(Guid id)
            => await Context.ConfigTypeValues.FirstOrDefaultAsync(p => p.Id == id);



        public async Task<List<ConfigTypeValues>> GetConfigTypeValuesByTypeIdAsync(Guid listTypeItemId)
            => await Context.ConfigTypeValues.Where(p => p.ListTypeItemId == listTypeItemId).ToListAsync();

        public async Task<Guid> CreateConfigTypeValuesAsync(ConfigTypeValues configTypeValues)
        {
            configTypeValues.CreatedOn = DateTime.Now;
            configTypeValues.UpdatedOn = DateTime.Now;
            configTypeValues.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            configTypeValues.CreatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.ConfigTypeValues.AddAsync(configTypeValues);
            await Context.SaveChangesAsync();
            return configTypeValues.Id;
        }

        public async Task<bool> DeleteConfigTypeValuesAsync(Guid id)
        {

            var deletedConfigTypeValues = await Context
                .ConfigTypeValues.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedConfigTypeValues == null) return true;

            //deletedConfigTypeValues.IsDeleted = true;
            //deletedConfigTypeValues.IsActive = false;

            Context.Entry(deletedConfigTypeValues).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateConfigTypeValuesAsync(ConfigTypeValues configTypeValues)
        {
            configTypeValues.UpdatedOn = DateTime.Now;
            configTypeValues.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(configTypeValues).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return configTypeValues.Id;
        }


    }
}