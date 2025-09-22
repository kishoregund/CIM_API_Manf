namespace Application.Features.Instruments.Commands
{
    public class DeleteInstrumentSparesCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid InstrumentSparesId { get; set; }
    }

    public class DeleteInstrumentSparesCommandHandler(IInstrumentSparesService InstrumentSparesService) : IRequestHandler<DeleteInstrumentSparesCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteInstrumentSparesCommand request, CancellationToken cancellationToken)
        {
            var deletedInstrumentSpares = await InstrumentSparesService.DeleteInstrumentSparesAsync(request.InstrumentSparesId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedInstrumentSpares, message: "InstrumentSpares deleted successfully.");
        }
    }
}
