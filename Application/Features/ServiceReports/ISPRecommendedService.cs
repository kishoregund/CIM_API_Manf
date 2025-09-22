using Domain.Views;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface ISPRecommendedService
    {
        Task<SPRecommended> GetSPRecommendedAsync(Guid id);
        Task<List<SPRecommended>> GetSPRecommendedBySRPIdAsync(Guid serviceReportId);
        Task<List<VW_SparesRecommended>> GetSPRecommendedGridAsync(string buId, string brandId);
        Task<Guid> CreateSPRecommendedAsync(SPRecommended SPRecommended);
        Task<Guid> UpdateSPRecommendedAsync(SPRecommended SPRecommended);
        Task<bool> DeleteSPRecommendedAsync(Guid id);
        Task<List<VW_Spareparts>> GetSPRecommendedBySerReqAsync(Guid ServiceRequestId);
    }
}