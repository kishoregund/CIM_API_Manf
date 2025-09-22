using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetAllRegionQuery : IRequest<IResponseWrapper>
    {
    }


    public class GetAllRegionQueryHandler(IRegionService regionService) : IRequestHandler<GetAllRegionQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAllRegionQuery request, CancellationToken cancellationToken)
        {
            var regionInDb = (await regionService.GetRegionsAsync()).Adapt<List<RegionResponse>>();

            if (regionInDb is not null)
            {
                return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(data: regionInDb);
            }
            return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(message: "Region does not exists.");
        }
    }
}