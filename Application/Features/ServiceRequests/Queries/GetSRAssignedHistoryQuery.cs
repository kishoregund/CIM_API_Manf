using Application.Features.ServiceRequests;
using Application.Features.ServiceRequests.Responses;

namespace Application.Features.SRAssignedHistorys.Queries
{
    public class GetSRAssignedHistoryQuery : IRequest<IResponseWrapper>
    {
        public Guid SRAssignedHistoryId { get; set; }
    }

    public class GetSRAssignedHistoryQueryHandler(ISRAssignedHistoryService SRAssignedHistoryService) : IRequestHandler<GetSRAssignedHistoryQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRAssignedHistoryQuery request, CancellationToken cancellationToken)
        {
            var SRAssignedHistoryInDb = (await SRAssignedHistoryService.GetSRAssignedHistoryAsync(request.SRAssignedHistoryId)).Adapt<SRAssignedHistoryResponse>();

            if (SRAssignedHistoryInDb is not null)
            {
                return await ResponseWrapper<SRAssignedHistoryResponse>.SuccessAsync(data: SRAssignedHistoryInDb);
            }
            return await ResponseWrapper<SRAssignedHistoryResponse>.SuccessAsync(message: "SRAssignedHistory does not exists.");
        }
    }
}