namespace Application.Features.Instruments.Commands
{
    public class DeleteInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid InstrumentId { get; set; }
    }

    public class DeleteInstrumentCommandHandler(IInstrumentService InstrumentService) : IRequestHandler<DeleteInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteInstrumentCommand request, CancellationToken cancellationToken)
        {
            var deletedInstrument = await InstrumentService.DeleteInstrumentAsync(request.InstrumentId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedInstrument, message: "Instrument deleted successfully.");
        }
    }
}
