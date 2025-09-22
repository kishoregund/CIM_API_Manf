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
using System.Linq.Expressions;
using Domain.Views;

namespace Infrastructure.Services
{
    public class InstrumentSparesService(ApplicationDbContext Context, ICurrentUserService currentUserService) : IInstrumentSparesService
    {

        public async Task<InstrumentSpares> GetInstrumentSparesAsync(Guid id)
            => await Context.InstrumentSpares.FirstOrDefaultAsync(p => p.Id == id);


        public async Task<List<InstrumentSpares>> GetInstrumentSparesEntityByInsIdAsync(Guid instrumentId)
           => await Context.InstrumentSpares.Where(p => p.InstrumentId == instrumentId).ToListAsync();

        public async Task<List<VW_InstrumentSpares>> GetInstrumentSparesByInsIdAsync(Guid instrumentId)
           => await Context.VW_InstrumentSpares.Where(p => p.InstrumentId == instrumentId).ToListAsync();

        public async Task<bool> CreateInstrumentSparesAsync(List<InstrumentSpares> listInstrumentSpares)
        {
            foreach (InstrumentSpares spare in listInstrumentSpares)
            {
                spare.CreatedOn = DateTime.Now;
                spare.UpdatedOn = DateTime.Now;
                spare.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                spare.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            }

            await Context.InstrumentSpares.AddRangeAsync(listInstrumentSpares);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInstrumentSparesAsync(Guid id)
        {

            var deletedInstrumentSpares = await Context.InstrumentSpares.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedInstrumentSpares == null) return true;

            deletedInstrumentSpares.IsDeleted = true;
            deletedInstrumentSpares.IsActive = false;

            Context.Entry(deletedInstrumentSpares).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid> UpdateInstrumentSparesAsync(InstrumentSpares InsSpare)
        {
            //foreach (InstrumentSpares spare in instrumentSpares)
            //{
            var spare = await Context.InstrumentSpares.FirstOrDefaultAsync(x => x.SparepartId == InsSpare.SparepartId && x.InstrumentId == InsSpare.SparepartId);
            spare.InsQty = InsSpare.InsQty;
            spare.UpdatedOn = DateTime.Now;
            spare.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            //}

            Context.Entry(spare).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return spare.Id;
        }

        public async Task<bool> UpdateInsertInstrumentSparesAsync(List<InstrumentSpares> listInstrumentSpares)
        {
            List<InstrumentSpares> lstInsert = new();
            foreach (InstrumentSpares spare in listInstrumentSpares)
            {
                if (Context.InstrumentSpares.Any(x => x.SparepartId == spare.SparepartId && x.InstrumentId == spare.InstrumentId))
                {
                    //spare.InsQty = spare.InsQty;
                    //spare.UpdatedOn = DateTime.Now;
                    //spare.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
                    //Context.Entry(spare).State = EntityState.Modified;
                }
                else
                {
                    lstInsert.Add(spare);
                }
            }
            await Context.SaveChangesAsync();

            await CreateInstrumentSparesAsync(lstInsert);
            return true;
        }
    }
}