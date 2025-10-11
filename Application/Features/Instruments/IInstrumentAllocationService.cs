
using Application.Features.Instruments.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instruments
{
    public interface IInstrumentAllocationService
    {
        Task<InstrumentAllocation> GetInstrumentAllocationEntityAsync(Guid id);
        Task<List<InstrumentAllocationResponse>> GetInstrumentAllocationsAsync();
        Task<Guid> CreateInstrumentAllocationAsync(InstrumentAllocation InstrumentAllocation);
        Task<Guid> UpdateInstrumentAllocationAsync(InstrumentAllocation InstrumentAllocation);
        Task<bool> DeleteInstrumentAllocationAsync(Guid id);
        Task<bool> IsDuplicateAsync(Guid instrumentId, Guid distributorId, Guid businessUnitId);
    }
}
