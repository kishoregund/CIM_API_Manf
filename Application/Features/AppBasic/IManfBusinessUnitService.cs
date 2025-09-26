namespace Application.Features.AppBasic
{
    public interface IManfBusinessUnitService
    {
        Task<List<ManfBusinessUnit>> GetManfBusinessUnitsAsync();
        Task<ManfBusinessUnit> GetManfBusinessUnitByIdAsync(Guid id);
        Task<Guid> CreateManfBusinessUnitAsync(ManfBusinessUnit ManfBusinessUnit);
        Task<bool> DeleteManfBusinessUnitAsync(Guid requestId);
        Task<Guid> UpdateManfBusinessUnitAsync(ManfBusinessUnit ManfBusinessUnit);
        Task<bool> IsDuplicateAsync(string buName);
    }
}