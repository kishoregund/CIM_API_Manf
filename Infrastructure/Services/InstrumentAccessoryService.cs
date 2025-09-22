
using Application.Features.Instruments;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class InstrumentAccessoryService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IInstrumentAccessoryService
    {

        public async Task<InstrumentAccessory> GetInstrumentAccessoryAsync(Guid id)
            => await Context.InstrumentAccessory.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<InstrumentAccessory>> GetInstrumentAccessoryByInsIdAsync(Guid instrumentId)
            => await Context.InstrumentAccessory.Where(p => p.InstrumentId == instrumentId).ToListAsync();

        public async Task<Guid> CreateInstrumentAccessoryAsync(InstrumentAccessory InstrumentAccessory)
        {
           InstrumentAccessory.CreatedOn = DateTime.Now;
           InstrumentAccessory.UpdatedOn = DateTime.Now;
           InstrumentAccessory.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            InstrumentAccessory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            await Context.InstrumentAccessory.AddAsync(InstrumentAccessory);
            await Context.SaveChangesAsync();
            return InstrumentAccessory.Id;
        }

        public async Task<bool> DeleteInstrumentAccessoryAsync(Guid id)
        {

            var deletedInstrumentAccessory = await Context.InstrumentAccessory.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedInstrumentAccessory == null) return true;

            //deletedInstrumentAccessory.IsDeleted = true;
            //deletedInstrumentAccessory.IsActive = false;

            Context.Entry(deletedInstrumentAccessory).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateInstrumentAccessoryAsync(InstrumentAccessory InstrumentAccessory)
        {
            InstrumentAccessory.UpdatedOn = DateTime.Now;
            InstrumentAccessory.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            Context.Entry(InstrumentAccessory).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return InstrumentAccessory.Id;
        }
    }
}