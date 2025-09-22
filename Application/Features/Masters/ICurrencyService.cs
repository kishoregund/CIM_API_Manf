namespace Application.Features.Masters
{
    public interface ICurrencyService
    {
        Task<Currency> GetCurrencyAsync(Guid id);
        Task<List<Currency>> GetCurrenciesAsync();
        Task<Guid> CreateCurrencyAsync(Currency currency);
        Task<Guid> UpdateCurrencyAsync(Currency currency);
        Task<bool> DeleteCurrencyAsync(Guid id);
    }
}
