namespace Application.Features.AppBasic
{
    public interface IBusinessUnitService
    {
        Task<List<BusinessUnit>> GetBusinessUnitsAsync();
        Task<BusinessUnit> GetBusinessUnitByIdAsync(Guid id);
        Task<Guid> CreateBusinessUnitAsync(BusinessUnit businessUnit);
        Task<bool> DeleteBusinessUnitAsync(Guid requestId);
        Task<Guid> UpdateBusinessUnitAsync(BusinessUnit businessUnit);
        Task<bool> IsDuplicateAsync(string buName);
    }
}