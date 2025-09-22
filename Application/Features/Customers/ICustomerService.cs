
using Application.Features.Customers.Responses;

namespace Application.Features.Customers
{
    public interface ICustomerService
    {
        Task<Domain.Entities.Customer> GetCustomerAsync(Guid id);
        Task<List<CustomerResponse>> GetCustomersAsync();
        Task<List<CustomerResponse>> GetCustomersByUserIdAsync(Guid userId);
        Task<Guid> CreateCustomerAsync(Domain.Entities.Customer Customer);
        Task<Guid> UpdateCustomerAsync(Domain.Entities.Customer Customer);
        Task<bool> DeleteCustomerAsync(Guid id);
        Task<bool> IsDuplicateAsync(string custName);
    }
}
