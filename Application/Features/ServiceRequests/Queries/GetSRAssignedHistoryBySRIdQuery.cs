using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSRAssignedHistoryBySRIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetSRAssignedHistoryBySRIdQueryHandler(ISRAssignedHistoryService SRAssignedHistoryBySRIdervice) : IRequestHandler<GetSRAssignedHistoryBySRIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRAssignedHistoryBySRIdQuery request, CancellationToken cancellationToken)
        {
            var SRAssignedHistoryBySRIdInDb = await SRAssignedHistoryBySRIdervice.GetSRAssignedHistoryBySRIdAsync(request.ServiceRequestId);

            if (SRAssignedHistoryBySRIdInDb.Count > 0)
            {
                return await ResponseWrapper<List<SRAssignedHistoryResponse>>.SuccessAsync(data: SRAssignedHistoryBySRIdInDb.Adapt<List<SRAssignedHistoryResponse>>());
            }
            return await ResponseWrapper<List<SRAssignedHistoryResponse>>.SuccessAsync(message: "No SRAssignedHistory were found.");
        }
    }
}
