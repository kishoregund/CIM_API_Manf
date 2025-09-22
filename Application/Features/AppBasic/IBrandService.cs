using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic
{
    public interface IBrandService
    {
        Task<Brand> GetBrandByIdAsync(Guid id);
        Task<List<BrandResponse>> GetBrandsByBusinessUnitAsync(Guid businessUnitId);
        Task<List<BrandResponse>> GetBrandsByBusinessUnitsAsync(string businessUnits);        
        Task<List<BrandResponse>> GetBrandsAsync();        
        Task<Guid> CreateBrandAsync(Brand brand);
        Task<Guid> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(Guid id);
        Task<bool> IsDuplicateAsync(string brandName, Guid businessUnitId);
    }
}
