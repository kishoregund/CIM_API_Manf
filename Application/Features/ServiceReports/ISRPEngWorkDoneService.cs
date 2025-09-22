
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface ISRPEngWorkDoneService
    {
        Task<SRPEngWorkDone> GetSRPEngWorkDoneAsync(Guid id);
        Task<List<SRPEngWorkDone>> GetSRPEngWorkDoneBySRPIdAsync(Guid serviceReportId);
        Task<Guid> CreateSRPEngWorkDoneAsync(SRPEngWorkDone SRPEngWorkDone);
        Task<Guid> UpdateSRPEngWorkDoneAsync(SRPEngWorkDone SRPEngWorkDone);
        Task<bool> DeleteSRPEngWorkDoneAsync(Guid id);
    }
}