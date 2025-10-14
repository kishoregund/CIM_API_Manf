

using Application.Features.Manufacturers.Responses;

namespace Application.Features.Manufacturers
{
    public interface IManufacturerService
    {
        Task<List<ManufacturerResponse>> GetManufacturersAsync();
        Task<Domain.Entities.Manufacturer> GetManufacturerAsync(Guid id);
        Task<Guid> CreateManufacturerAsync(Domain.Entities.Manufacturer Manufacturer);
        Task<Guid> UpdateManufacturerAsync(Domain.Entities.Manufacturer Manufacturer);
        Task<bool> DeleteManufacturerAsync(Guid id);
        Task<bool> IsDuplicateAsync(string manufacturerName);
        Task<bool> IsManfSubscribedAsync();
    }
}