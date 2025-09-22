using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetAllRegionContactQuery : IRequest<IResponseWrapper>
    {
        public Guid RegionId { get; set; } 
    }


    public class GetAllRegionContactQueryHandler(IRegionContactService regionContactService) : IRequestHandler<GetAllRegionContactQuery, IResponseWrapper>
    {        
        public async Task<IResponseWrapper> Handle(GetAllRegionContactQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await regionContactService.GetRegionContactsAsync(request.RegionId)).Adapt<List<RegionContactResponse>>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(message: "Region Contact does not exists.");
        }
    }
}