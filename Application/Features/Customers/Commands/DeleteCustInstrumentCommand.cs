
namespace Application.Features.Customers.Commands
{
    public class DeleteCustInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid CustInstrumentId { get; set; }
    }

    public class DeleteCustInstrumentCommandHandler(ICustInstrumentService CustInstrumentService) : IRequestHandler<DeleteCustInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteCustInstrumentCommand request, CancellationToken cancellationToken)
        {
            var deletedCustInstrument = await CustInstrumentService.DeleteCustomerInstrumentAsync(request.CustInstrumentId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedCustInstrument, message: "CustInstrument deleted successfully.");
        }
    }
}
