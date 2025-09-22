using Application.Features.Tenancy.Models;

namespace Application.Features.AppBasic
{
    public interface IAppBasicService
    {
        Task<ModalDataResponse> GetModalDataAsync();
    }
}
