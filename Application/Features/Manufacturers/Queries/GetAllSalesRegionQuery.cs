using Application.Features.Manufacturers.Responses;

namespace Application.Features.Manufacturers.Queries
{
    public class GetAllSalesRegionQuery : IRequest<IResponseWrapper>
    {
        public Guid ManufacturerId { get; set; }
    }

    public class GetAllSalesRegionQueryHandler(ISalesRegionService SalesRegionervice) : IRequestHandler<GetAllSalesRegionQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllSalesRegionQuery request, CancellationToken cancellationToken)
        {
            var SalesRegionInDb = await SalesRegionervice.GetSalesRegionsAsync(request.ManufacturerId);

            if (SalesRegionInDb.Count > 0)
            {
                return await ResponseWrapper<List<SalesRegionResponse>>.SuccessAsync(data: SalesRegionInDb.Adapt<List<SalesRegionResponse>>());
            }
            return await ResponseWrapper<List<SalesRegionResponse>>.SuccessAsync(message: "No SalesRegion were found.");
        }
    }
}
