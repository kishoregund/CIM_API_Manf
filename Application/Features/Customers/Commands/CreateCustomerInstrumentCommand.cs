using Application.Features.Customers.Requests;
using Application.Features.Customers.Responses;


namespace Application.Features.Customers.Commands
{
    public class CreateCustomerInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CustomerInstrumentRequest CustomerInstrumentRequest { get; set; }
    }

    public class CreateCustomerInstrumentCommandHandler(ICustInstrumentService CustomerInstrumentService) : IRequestHandler<CreateCustomerInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateCustomerInstrumentCommand request, CancellationToken cancellationToken)
        {
            // map

            var newCustomerInstrument = request.CustomerInstrumentRequest.Adapt<CustomerInstrument>();

            var CustomerInstrumentId = await CustomerInstrumentService.CreateCustomerInstrumentAsync(newCustomerInstrument);

            return await ResponseWrapper<Guid>.SuccessAsync(data: CustomerInstrumentId, message: "Record saved successfully.");
        }
    }
}

