using Application.Features.AMCS;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;
using System.Security.Cryptography;
using Application.Features.AMCS.Requests;
using Mapster;
using Application.Features.AMCS.Responses;

namespace Infrastructure.Services
{
    public class AmcInstrumentService(ApplicationDbContext context, ICurrentUserService currentUserService) : IAmcInstrumentService
    {
        public async Task<bool> CreateAmcInstrument(List<AMCInstrument> amcInstruments)
        {
            foreach (AMCInstrument amcInstrument in amcInstruments)
            {
                if (!context.AMCInstrument.Any(x => x.InstrumentId == amcInstrument.InstrumentId && x.AMCId == amcInstrument.AMCId))
                {
                    amcInstrument.IsActive = true;
                    amcInstrument.IsDeleted = false;
                    amcInstrument.CreatedOn = DateTime.Now;
                    amcInstrument.UpdatedOn = DateTime.Now;
                    amcInstrument.CreatedBy = Guid.Parse(currentUserService.GetUserId());
                    amcInstrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

                    await context.AMCInstrument.AddAsync(amcInstrument);
                }
            }
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAmcInstrument(Guid id)
        {
            var amcInstrument = await context.AMCInstrument.FirstOrDefaultAsync(x => x.Id == id);
            if (amcInstrument is null)
                return false;

            amcInstrument.IsDeleted = true;

            context.Entry(amcInstrument).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return false;
        }

        public async Task<List<AmcInstrumentResponse>> GetByAmcIdAsync(Guid requestAmcId)
        { 
           var instruments = (await context.AMCInstrument.Where(x => x.AMCId == requestAmcId).ToListAsync()).Adapt<List<AmcInstrumentResponse>>();
            foreach (var instr in instruments)
            {
                instr.InsType = context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == instr.InsTypeId).ItemName;
            }

            return instruments;
        }

        public async Task<AMCInstrument> GetByIdAsync(Guid requestId)
            => await context.AMCInstrument.FirstOrDefaultAsync(x => x.Id == requestId);

        public async Task<Guid> UpdateInstrumentAsync(AMCInstrument amcInstrument)
        {
            amcInstrument.UpdatedOn = DateTime.Now;
            amcInstrument.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            context.Entry(amcInstrument).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return amcInstrument.Id;
        }

        public async Task<bool> ExistsInstrumentInAMCQuery(AmcRequest mAmc)
        {
            var lstInstrument = mAmc.InstrumentIds;

            if (string.IsNullOrEmpty(lstInstrument)) return false;

            var amcInExists = await context.AMCInstrument.Where(x => x.InstrumentId.ToString() == lstInstrument).ToListAsync();

            if (amcInExists.Count > 0)
            {
                return (from amcIns in amcInExists
                        from x in context.AMC.ToList()
                        select x.Id == amcIns.AMCId && x.CustSite == mAmc.CustSite &&
                               ((DateTime.Compare(GetParsedDate(x.SDate), GetParsedDate(mAmc.SDate)) <= 0 &&
                               DateTime.Compare(GetParsedDate(x.EDate), GetParsedDate(mAmc.SDate)) >= 0) ||
                               (DateTime.Compare(GetParsedDate(x.SDate), GetParsedDate(mAmc.EDate)) <= 0 &&
                               DateTime.Compare(GetParsedDate(x.EDate), GetParsedDate(mAmc.EDate)) >= 0))
                       )
                    .Any(amcExists => amcExists);
            }

            return false;
        }

        private DateTime GetParsedDate(string date)
        {
            var dateRes = DateTime.ParseExact(date, "dd/MM/yyyy", null);
            return dateRes;
        }
    }
}