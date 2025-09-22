using Application.Features.AppBasic.Responses;
using Application.Features.Travels.Responses;

namespace Application.Features.Travels
{
    public interface IBankDetailsService
    {
        Task<BankDetails> GetBankDetailsEntityByIdAsync(Guid id);
        Task<BankDetails> GetBankDetailsByIdAsync(Guid id);
        Task<BankDetails> GetBankDetailsByContactIdAsync(Guid contactId);
        Task<Guid> CreateBankDetailsAsync(BankDetails BankDetails);
        Task<Guid> UpdateBankDetailsAsync(BankDetails BankDetails);
        Task<bool> DeleteBankDetailsAsync(Guid id);
    }
}
