using Application.Features.Spares.Responses;
using Domain.Views;

namespace Application.Features.Spares
{
    public interface ISparepartService
    {
        Task<VW_Spareparts> GetSparepartAsync(Guid id);
        Task<Sparepart> GetSparepartEntityAsync(Guid id);        
        Task<List<VW_Spareparts>> GetSparepartsAsync();
        Task<List<VW_Spareparts>> GetConfigSparepartAsync(Guid configTypeId);//, Guid configValueId);
        Task<VW_Spareparts> GetSparepartByPartNoAsync(string partNo);
        Task<Guid> CreateSparepartAsync(Sparepart Sparepart);
        Task<Guid> UpdateSparepartAsync(Sparepart Sparepart);
        Task<bool> DeleteSparepartAsync(Guid id);
    }
}
