using Application.Features.Identity.Users;
using Application.Features.Instruments;
using Application.Features.Instruments.Responses;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class InstrumentAllocationService(ApplicationDbContext context, ICurrentUserService currentUserService) : IInstrumentAllocationService
    {
        public async Task<List<InstrumentAllocationResponse>> GetInstrumentAllocationsAsync()
        {
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId.ToString() == currentUserService.GetUserId());
            var InstrumentAllocations = await context.InstrumentAllocation.ToListAsync();
            if (userProfile != null && userProfile.ContactType == "DR")
            {
                var bIds = userProfile.BusinessUnitIds.Split(',');
                InstrumentAllocations = InstrumentAllocations.Where(x => bIds.Contains(x.BusinessUnitId.ToString())).ToList();
            }
            if (userProfile != null && userProfile.ContactType == "MSR")
            {
                var bIds = userProfile.ManfBUIds.Split(',');
                InstrumentAllocations = (from d in context.Distributor.Where(x => bIds.Contains(x.ManfBusinessUnitId.ToString())).ToList()
                                         join ia in InstrumentAllocations on d.Id equals ia.DistributorId
                                         select ia).ToList();
            }

            return (from b in InstrumentAllocations
                    join bu in context.BusinessUnit on b.BusinessUnitId equals bu.Id
                    join ins in context.Instrument on b.InstrumentId equals ins.Id
                    join d in context.Distributor on b.DistributorId equals d.Id
                    join br in context.Brand on b.BrandId equals br.Id
                    select new InstrumentAllocationResponse
                    {
                        BusinessUnit = bu.BusinessUnitName,
                        BusinessUnitId = b.BusinessUnitId,
                        BrandId = b.BrandId,
                        BrandName = br.BrandName,
                        Instrument = ins.SerialNos,
                        InstrumentId = ins.Id,
                        DistributorId = d.Id,
                        DistributorName = d.DistName,
                        CreatedBy = b.CreatedBy,
                        CreatedOn = b.CreatedOn,
                        Id = b.Id,
                        IsActive = b.IsActive,
                        IsDeleted = b.IsDeleted,
                        UpdatedBy = b.UpdatedBy,
                        UpdatedOn = b.UpdatedOn
                    }).ToList();
        }

        public async Task<InstrumentAllocation> GetInstrumentAllocationEntityAsync(Guid id)
            => await context.InstrumentAllocation.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<InstrumentAllocation> GetInstrumentAllocationByInsIdAsync(Guid insId)
            => await context.InstrumentAllocation.FirstOrDefaultAsync(x => x.InstrumentId == insId);

        public async Task<Guid> CreateInstrumentAllocationAsync(InstrumentAllocation InstrumentAllocation)
        {
            InstrumentAllocation.CreatedOn = DateTime.Now;
            InstrumentAllocation.UpdatedOn = DateTime.Now;
            InstrumentAllocation.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            InstrumentAllocation.CreatedBy = Guid.Parse(currentUserService.GetUserId());

            await context.InstrumentAllocation.AddAsync(InstrumentAllocation);
            await context.SaveChangesAsync();
            return InstrumentAllocation.Id;
        }

        public async Task<Guid> UpdateInstrumentAllocationAsync(InstrumentAllocation InstrumentAllocation)
        {
            InstrumentAllocation.UpdatedOn = DateTime.Now;
            InstrumentAllocation.UpdatedBy = Guid.Parse(currentUserService.GetUserId());

            context.Entry(InstrumentAllocation).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return InstrumentAllocation.Id;
        }

        public async Task<bool> DeleteInstrumentAllocationAsync(Guid id)
        {

            var deleteInstrumentAllocation = await context
                .InstrumentAllocation.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteInstrumentAllocation == null) return true;

            context.Entry(deleteInstrumentAllocation).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateAsync(Guid instrumentId, Guid distributorId, Guid businessUnitId)
            => await context.InstrumentAllocation.AnyAsync(x => x.BusinessUnitId == businessUnitId && x.InstrumentId == instrumentId && x.DistributorId == distributorId);

    }
}
