
using Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instruments
{
    public interface IInstrumentSparesService
    {
        Task<InstrumentSpares> GetInstrumentSparesAsync(Guid id);
        Task<List<InstrumentSpares>> GetInstrumentSparesEntityByInsIdAsync(Guid instrumentId);
        Task<List<VW_InstrumentSpares>> GetInstrumentSparesByInsIdAsync(Guid instrumentId);        
        Task<bool> CreateInstrumentSparesAsync(List<InstrumentSpares> InstrumentSpares);
        Task<Guid> UpdateInstrumentSparesAsync(InstrumentSpares InstrumentSpares);
        Task<bool> UpdateInsertInstrumentSparesAsync(List<InstrumentSpares> listInstrumentSpares);
        Task<bool> DeleteInstrumentSparesAsync(Guid id);
    }
}
