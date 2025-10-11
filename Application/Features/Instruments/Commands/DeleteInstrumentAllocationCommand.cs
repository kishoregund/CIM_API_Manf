namespace Application.Features.Instruments.Commands
{
    public class DeleteInstrumentAllocationCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid InstrumentAllocationId { get; set; }
    }

    public class DeleteInstrumentAllocationCommandHandler(IInstrumentAllocationService InstrumentAllocationService) : IRequestHandler<DeleteInstrumentAllocationCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteInstrumentAllocationCommand request, CancellationToken cancellationToken)
        {
            var deletedInstrumentAllocation = await InstrumentAllocationService.DeleteInstrumentAllocationAsync(request.InstrumentAllocationId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedInstrumentAllocation, message: "InstrumentAllocation deleted successfully.");
        }
    }
}
