using Application.Features.ServiceRequests;

namespace Application.Features.SRAssignedHistorys.Commands
{
    public class DeleteSRAssignedHistoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid SRAssignedHistoryId { get; set; }
    }

    public class DeleteSRAssignedHistoryCommandHandler(ISRAssignedHistoryService SRAssignedHistoryService) : IRequestHandler<DeleteSRAssignedHistoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteSRAssignedHistoryCommand request, CancellationToken cancellationToken)
        {
            var deletedSRAssignedHistory = await SRAssignedHistoryService.DeleteSRAssignedHistoryAsync(request.SRAssignedHistoryId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedSRAssignedHistory, message: "SRAssignedHistory deleted successfully.");
        }
    }
}
