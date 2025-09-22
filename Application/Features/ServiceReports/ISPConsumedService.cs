
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface ISPConsumedService
    {
        Task<SPConsumed> GetSPConsumedAsync(Guid id);
        Task<List<SPConsumed>> GetSPConsumedBySRPIdAsync(Guid serviceReportId);
        Task<Guid> CreateSPConsumedAsync(SPConsumed SPConsumed);
        Task<Guid> UpdateSPConsumedAsync(SPConsumed SPConsumed);
        Task<bool> DeleteSPConsumedAsync(Guid id);
    }
}