
using Application.Features.ServiceRequests.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface ISREngActionService
    {
        Task<SREngAction> GetSREngActionAsync(Guid id);
        Task<List<SREngActionResponse>> GetSREngActionBySRIdAsync(Guid serviceRequestId);
        Task<List<SREngAction>> GetSREngActionEntityBySRIdAsync(Guid serviceRequestId);
        Task<Guid> CreateSREngActionAsync(SREngAction SREngAction);
        Task<Guid> UpdateSREngActionAsync(SREngAction SREngAction);
        Task<bool> DeleteSREngActionAsync(Guid id);
    }
}