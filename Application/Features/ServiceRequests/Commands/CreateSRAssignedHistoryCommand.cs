using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateSRAssignedHistoryCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public SRAssignedHistoryRequest SRAssignedHistoryRequest { get; set; }
    }

    public class CreateSrAssignedHistoryCommandHandler(ISRAssignedHistoryService SrAssignedHistoryService) : IRequestHandler<CreateSRAssignedHistoryCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateSRAssignedHistoryCommand request, CancellationToken cancellationToken)
        {
            // map

            var newSrAssignedHistory = request.SRAssignedHistoryRequest.Adapt<SRAssignedHistory>();

            var SrAssignedHistoryId = await SrAssignedHistoryService.CreateSRAssignedHistoryAsync(newSrAssignedHistory);

            return await ResponseWrapper<Guid>.SuccessAsync(data: SrAssignedHistoryId, message: "Record saved successfully.");
        }
    }
}
