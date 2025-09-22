
using Application.Features.ServiceRequests.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface ISRAssignedHistoryService
    {
        Task<SRAssignedHistory> GetSRAssignedHistoryAsync(Guid id);
        Task<List<SRAssignedHistoryResponse>> GetSRAssignedHistoryBySRIdAsync(Guid serviceRequestId);
        Task<List<SRAssignedHistory>> GetSRAssignedHistoryEntityBySRIdAsync(Guid serviceRequestId);
        Task<Guid> CreateSRAssignedHistoryAsync(SRAssignedHistory SRAssignedHistory);
        Task<Guid> UpdateSRAssignedHistoryAsync(SRAssignedHistory SRAssignedHistory);
        Task<bool> DeleteSRAssignedHistoryAsync(Guid id);
    }
}