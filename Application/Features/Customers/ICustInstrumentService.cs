
using Application.Features.Customers.Requests;
using Application.Features.Customers.Responses;
using Application.Features.Instruments.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers
{
    public interface ICustInstrumentService
    {
        Task<CustomerInstrument> GetCustomerInstrumentAsync(Guid id);
        Task<List<CustomerInstrument>> GetCustomerInstrumentsAsync(Guid customerId);
        Task<CustomerInstrument> GetCustomerInstrumentByInstrumentAsync(Guid instrumentId);
        Task<List<InstrumentResponse>> GetCustomersInstrumentBySiteAsync(Guid siteId);
        Task<List<CustomerInstrumentResponse>> GetAssignedCustomerInstrumentsAsync(string businessUnitId, string brandId);        
        Task<Guid> CreateCustomerInstrumentAsync(CustomerInstrument CustomerInstrument);
        Task<Guid> UpdateCustomerInstrumentAsync(CustomerInstrument CustomerInstrument);
        Task<bool> DeleteCustomerInstrumentAsync(Guid id);
        Task<bool> IsDuplicateAsync(CustomerInstrumentRequest customerInstrument);
    }
}
