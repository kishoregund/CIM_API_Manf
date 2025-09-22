namespace Application.Features.Masters
{
    public interface ICountryService
    {
        Task<Country> GetCountryAsync(Guid id);
        Task<List<Country>> GetCountriesAsync();
        Task<Guid> CreateCountryAsync(Country country);
        Task<Guid> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Guid id);
    }
}
