using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetSRAuditTrailBySRIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetSRAuditTrailBySRIdQueryHandler(ISRAuditTrailService SRAuditTrailByService) : IRequestHandler<GetSRAuditTrailBySRIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRAuditTrailBySRIdQuery request, CancellationToken cancellationToken)
        {
            var SRAuditTrailInDb = await SRAuditTrailByService.GetSRAuditTrailBySRIdAsync(request.ServiceRequestId);

            if (SRAuditTrailInDb.Count > 0)
            {
                return await ResponseWrapper<List<SRAuditTrailResponse>>.SuccessAsync(data: SRAuditTrailInDb.Adapt<List<SRAuditTrailResponse>>());
            }
            return await ResponseWrapper<List<SRAuditTrailResponse>>.SuccessAsync(message: "No SRAuditTrail were found.");
        }
    }
}
