using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetAssignedRegionsQuery : IRequest<IResponseWrapper>
    {
    }


    public class GetAssignedRegionsQueryHandler(IRegionService regionService) : IRequestHandler<GetAssignedRegionsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetAssignedRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionInDb = (await regionService.GetAssignedRegionsAsync()).Adapt<List<RegionResponse>>();

            if (regionInDb is not null)
            {
                return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(data: regionInDb);
            }
            return await ResponseWrapper<List<RegionResponse>>.SuccessAsync(message: "Region does not exists.");
        }
    }
}