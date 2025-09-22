using Application.Features.Customers.Commands;
using Application.Features.Customers;
using Application.Features.Customers.Requests;
using Domain.Entities;
using System.IO;

namespace Application.Features.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerRequest CustomerRequest { get; set; }
    }

    public class UpdateCustomerCommandHandler(ICustomerService CustomerService) : IRequestHandler<UpdateCustomerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var CustomerInDb = await CustomerService.GetCustomerAsync(request.CustomerRequest.Id);

            CustomerInDb.Id = request.CustomerRequest.Id;
            CustomerInDb.Code = request.CustomerRequest.Code;
            CustomerInDb.CustName = request.CustomerRequest.CustName;
            CustomerInDb.DefDistId = request.CustomerRequest.DefDistId;
            CustomerInDb.DefDistRegionId = request.CustomerRequest.DefDistRegionId;
            CustomerInDb.IndustrySegment = request.CustomerRequest.IndustrySegment;
            CustomerInDb.CountryId = request.CustomerRequest.CountryId;
            CustomerInDb.Street = request.CustomerRequest.Street;
            CustomerInDb.Place = request.CustomerRequest.Place;
            CustomerInDb.Area = request.CustomerRequest.Area;
            CustomerInDb.City = request.CustomerRequest.City;
            CustomerInDb.AddrCountryId = request.CustomerRequest.AddrCountryid;
            CustomerInDb.GeoLat = request.CustomerRequest.GeoLat;
            CustomerInDb.GeoLong = request.CustomerRequest.GeoLong;
            CustomerInDb.UpdatedBy = request.CustomerRequest.UpdatedBy;

            var updateCustomerId = await CustomerService.UpdateCustomerAsync(CustomerInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateCustomerId, message: "Record updated successfully.");
        }
    }
}
