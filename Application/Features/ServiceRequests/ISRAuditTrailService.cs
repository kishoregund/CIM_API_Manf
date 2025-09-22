
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface ISRAuditTrailService
    {
        Task<SRAuditTrail> GetSRAuditTrailAsync(Guid id);
        Task<List<SRAuditTrail>> GetSRAuditTrailBySRIdAsync(Guid serviceRequestId);
        Task<Guid> CreateSRAuditTrailAsync(SRAuditTrail SRAuditTrail);
        Task<Guid> UpdateSRAuditTrailAsync(SRAuditTrail SRAuditTrail);
        Task<bool> DeleteSRAuditTrailAsync(Guid id);
    }
}