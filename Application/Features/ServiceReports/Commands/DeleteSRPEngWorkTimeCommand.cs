namespace Application.Features.ServiceReports.Commands
{
    public class DeleteSRPEngWorkTimeCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SRPEngWorkTimeId { get; set; }
    }

    public class DeleteSRPEngWorkTimeCommandHandler(ISRPEngWorkTimeService SRPEngWorkTimeService) : IRequestHandler<DeleteSRPEngWorkTimeCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSRPEngWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var deletedSRPEngWorkTime = await SRPEngWorkTimeService.DeleteSRPEngWorkTimeAsync(request.SRPEngWorkTimeId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSRPEngWorkTime, message: "SRPEngWorkTime deleted successfully.");
        }
    }
}
