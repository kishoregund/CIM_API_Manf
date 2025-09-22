using Application.Features.ServiceRequests.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests
{
    public interface ISREngCommentsService
    {
        Task<SREngComments> GetSREngCommentAsync(Guid id);
        Task<List<SREngCommentsResponse>> GetSREngCommentBySRIdAsync(Guid serviceRequestId);
        Task<List<SREngComments>> GetSREngCommentEntityBySRIdAsync(Guid serviceRequestId);
        Task<Guid> CreateSREngCommentAsync(SREngComments SREngComment);
        Task<Guid> UpdateSREngCommentAsync(SREngComments SREngComment);
        Task<bool> DeleteSREngCommentAsync(Guid id);
    }
}
