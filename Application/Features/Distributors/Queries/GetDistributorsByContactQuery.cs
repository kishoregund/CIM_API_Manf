using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetDistributorsByContactQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetDistributorsByContactQueryHandler(IDistributorService distributorService, IRegionService regionService) : IRequestHandler<GetDistributorsByContactQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDistributorsByContactQuery request, CancellationToken cancellationToken)
        {
            var distributorInDb = (await distributorService.GetDistributorsByContactAsync(request.ContactId));
            foreach (DistributorResponse dist in distributorInDb)
            {
                dist.Regions = (await regionService.GetRegionsByDistributorAsync(dist.Id)).Adapt<List<RegionResponse>>();
            }
            if (distributorInDb.Count > 0)
            {
                return await ResponseWrapper<List<DistributorResponse>>.SuccessAsync(data: distributorInDb);
            }
            return await ResponseWrapper<DistributorResponse>.SuccessAsync(message: "Distributor does not exists.");
        }
    }
}