using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic
{
    public interface IBusinessUnitService
    {
        Task<List<BusinessUnitResponse>> GetBusinessUnitsAsync();
        Task<List<BusinessUnit>> GetBusinessUnitsByDistributorAsync(Guid distributorId);
        Task<BusinessUnit> GetBusinessUnitByIdAsync(Guid id);
        Task<Guid> CreateBusinessUnitAsync(BusinessUnit businessUnit);
        Task<bool> DeleteBusinessUnitAsync(Guid requestId);
        Task<Guid> UpdateBusinessUnitAsync(BusinessUnit businessUnit);
        Task<bool> IsDuplicateAsync(string buName);
    }
}