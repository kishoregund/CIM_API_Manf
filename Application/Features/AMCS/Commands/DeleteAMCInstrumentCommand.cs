namespace Application.Features.AMCS.Commands
{
    public class DeleteAmcInstrumentCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid Id { get; set; } 
    }

    public class DeleteAmcInstrumentCommandHandler(IAmcInstrumentService amcInstrumentService)
        : IRequestHandler<DeleteAmcInstrumentCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteAmcInstrumentCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await amcInstrumentService.DeleteAmcInstrument(request.Id);

            return await ResponseWrapper<bool>.SuccessAsync(data: isDeleted, message: "AMC Instrument deleted successfully.");
        }
    }

}
