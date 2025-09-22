using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetAllRegionbyDistributorQuery : IRequest<IResponseWrapper>
    {
        public Guid DistributorId { get; set; }
    }


    public class GetAllRegionbyDistributorQueryHandler(IRegionService regionService) : IRequestHandler<GetAllRegionbyDistributorQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllRegionbyDistributorQuery request, CancellationToken cancellationToken)
        {
            var regionInDb = (await regionService.GetRegionsByDistributorAsync(request.DistributorId)).Adapt<List<RegionResponse>>();

            if (regionInDb is not null)
            {
                return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(data: regionInDb);
            }
            return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(message: "Region does not exists.");
        }
    }
}