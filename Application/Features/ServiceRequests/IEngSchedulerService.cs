using Application.Features.ServiceRequests.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface IEngSchedulerService
    {
        Task<EngScheduler> GetEngSchedulerAsync(Guid id);
        Task<List<EngSchedulerResponse>> GetEngSchedulerBySRIdAsync(Guid serviceRequestId);
        Task<List<EngSchedulerResponse>> GetEngSchedulerByEngineerAsync(Guid engineerId);
        Task<List<EngScheduler>> GetEngSchedulerEntityBySRIdAsync(Guid ServiceRequestId);
        Task<Guid> CreateEngSchedulerAsync(EngScheduler EngScheduler);
        Task<Guid> UpdateEngSchedulerAsync(EngScheduler EngScheduler);
        Task<bool> DeleteEngSchedulerAsync(Guid id);
    }
}
