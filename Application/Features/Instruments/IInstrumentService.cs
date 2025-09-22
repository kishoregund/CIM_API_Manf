
using Application.Features.Instruments.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instruments
{
    public interface IInstrumentService
    {
        Task<Domain.Entities.Instrument> GetInstrumentEntityAsync(Guid id);
        Task<InstrumentResponse> GetInstrumentAsync(Guid id);
        Task<InstrumentResponse> GetInstrumentBySerialNoAsync(string serialNo);
        Task<List<InstrumentResponse>> GetInstrumentsAsync(string businessUnitId, string brandId);
        Task<Guid> CreateInstrumentAsync(Domain.Entities.Instrument Instrument);
        Task<Guid> UpdateInstrumentAsync(Domain.Entities.Instrument Instrument);
        Task<bool> DeleteInstrumentAsync(Guid id);
        Task<bool> IsDuplicateAsync(string insType , string serialNos);
    }
}
