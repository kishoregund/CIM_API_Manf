
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instruments
{
    public interface IInstrumentAccessoryService
    {
        Task<InstrumentAccessory> GetInstrumentAccessoryAsync(Guid id);
        Task<List<InstrumentAccessory>> GetInstrumentAccessoryByInsIdAsync(Guid instrumentId);
        Task<Guid> CreateInstrumentAccessoryAsync(InstrumentAccessory InstrumentAccessory);
        Task<Guid> UpdateInstrumentAccessoryAsync(InstrumentAccessory InstrumentAccessory);
        Task<bool> DeleteInstrumentAccessoryAsync(Guid id);
    }
}
