using Application.Features.Customers.Responses;

namespace Application.Features.Customers.Queries
{
    public class GetDistRegionsByCustomerQuery : IRequest<IResponseWrapper>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetDistRegionsByCustomerQueryHandler(ISiteService SiteService) : IRequestHandler<GetDistRegionsByCustomerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDistRegionsByCustomerQuery request, CancellationToken cancellationToken)
        {
            var DRegionInDb = await SiteService.GetDistRegionsByCustomerAsync(request.CustomerId);

            if (DRegionInDb is not null)
            {
                return await ResponseWrapper<List<Regions>>.SuccessAsync(data: DRegionInDb);
            }
            return await ResponseWrapper<Regions>.SuccessAsync(message: "Distributor Region does not exists.");
        }
    }
}
