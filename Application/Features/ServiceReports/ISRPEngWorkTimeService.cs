
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface ISRPEngWorkTimeService
    {
        Task<SRPEngWorkTime> GetSRPEngWorkTimeAsync(Guid id);
        Task<List<SRPEngWorkTime>> GetSRPEngWorkTimeBySRPIdAsync(Guid serviceReportId);
        Task<Guid> CreateSRPEngWorkTimeAsync(SRPEngWorkTime SRPEngWorkTime);
        Task<Guid> UpdateSRPEngWorkTimeAsync(SRPEngWorkTime SRPEngWorkTime);
        Task<bool> DeleteSRPEngWorkTimeAsync(Guid id);
    }
}