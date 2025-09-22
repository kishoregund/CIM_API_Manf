
namespace Application.Features.Instruments.Commands
{
    public class DeleteInstrumentAccessoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid InstrumentAccessoryId { get; set; }
    }

    public class DeleteInstrumentAccessoryCommandHandler(IInstrumentAccessoryService InstrumentAccessoryService) : IRequestHandler<DeleteInstrumentAccessoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteInstrumentAccessoryCommand request, CancellationToken cancellationToken)
        {
            var deletedInstrumentAccessory = await InstrumentAccessoryService.DeleteInstrumentAccessoryAsync(request.InstrumentAccessoryId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedInstrumentAccessory, message: "InstrumentAccessory deleted successfully.");
        }
    }
}
