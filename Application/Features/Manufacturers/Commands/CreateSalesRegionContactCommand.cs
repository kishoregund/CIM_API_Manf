using Application.Features.Manufacturers.Requests;

namespace Application.Features.Manufacturers.Commands
{
    public class CreateSalesRegionContactCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SalesRegionContactRequest SalesRegionContactRequest { get; set; }
    }

    public class CreateSalesRegionContactCommandHandler(ISalesRegionContactService SalesRegionContactService) : IRequestHandler<CreateSalesRegionContactCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSalesRegionContactCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSalesRegionContact = request.SalesRegionContactRequest.Adapt<SalesRegionContact>();

            var SalesRegionContactId = await SalesRegionContactService.CreateSalesRegionContactAsync(newSalesRegionContact);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SalesRegionContactId, message: "Record saved successfully.");
        }
    }
}
