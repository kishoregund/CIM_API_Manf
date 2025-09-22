namespace Application.Features.ServiceReports.Commands
{
    public class DeleteSRPEngWorkDoneCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SRPEngWorkDoneId { get; set; }
    }

    public class DeleteSRPEngWorkDoneCommandHandler(ISRPEngWorkDoneService SRPEngWorkDoneService) : IRequestHandler<DeleteSRPEngWorkDoneCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSRPEngWorkDoneCommand request, CancellationToken cancellationToken)
        {
            var deletedSRPEngWorkDone = await SRPEngWorkDoneService.DeleteSRPEngWorkDoneAsync(request.SRPEngWorkDoneId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSRPEngWorkDone, message: "SRPEngWorkDone deleted successfully.");
        }
    }
}
