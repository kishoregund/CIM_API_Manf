using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetDistributorByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid DistributorId { get; set; }
    }


    public class GetDistributorByIdQueryHandler(IDistributorService distributorService, IRegionService regionService) : IRequestHandler<GetDistributorByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDistributorByIdQuery request, CancellationToken cancellationToken)
        {
            var distributorInDb = await distributorService.GetDistributorAsync(request.DistributorId);

            distributorInDb.Regions = (await regionService.GetRegionsByDistributorAsync(request.DistributorId)).Adapt<List<RegionResponse>>();

            if (distributorInDb is not null)
            {
                return await ResponseWrapper<DistributorResponse>.SuccessAsync(data: distributorInDb);
            }
            return await ResponseWrapper<DistributorResponse>.SuccessAsync(message: "Distributor does not exists.");
        }
    }
}