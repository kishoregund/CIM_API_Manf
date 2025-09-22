using Application.Features.Travels.Responses;

namespace Application.Features.Travels.Queries
{
    public class GetAdvanceRequestsQuery : IRequest<IResponseWrapper>
    {
        public string BusinessUnitId { get; set; }
        public string BrandId { get; set; }
    }

    public class GetAdvanceRequestsQueryHandler(IAdvanceRequestService AdvanceRequestservice) : IRequestHandler<GetAdvanceRequestsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAdvanceRequestsQuery request, CancellationToken cancellationToken)
        {
            var AdvanceRequestsInDb = await AdvanceRequestservice.GetAdvanceRequestsAsync(request.BusinessUnitId, request.BrandId);

            if (AdvanceRequestsInDb.Count > 0)
            {
                return await ResponseWrapper<List<AdvanceRequestResponse>>.SuccessAsync(data: AdvanceRequestsInDb.Adapt<List<AdvanceRequestResponse>>());
            }
            return await ResponseWrapper<List<AdvanceRequestResponse>>.SuccessAsync(message: "No AdvanceRequests were found.");
        }
    }
}