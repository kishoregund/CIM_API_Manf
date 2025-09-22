using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetRegionContactByDistIdQuery : IRequest<IResponseWrapper>
    {
        public Guid DistributorId { get; set; } 
        public string Code { get; set; }
    }


    public class GetRegionContactByDistIdQueryHandler(IRegionContactService regionContactService) : IRequestHandler<GetRegionContactByDistIdQuery, IResponseWrapper>
    {        
        public async Task<IResponseWrapper> Handle(GetRegionContactByDistIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await regionContactService.GetDistributorRegionEngineers(request.DistributorId, request.Code)).Adapt<List<RegionContactResponse>>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(message: "Region Contact does not exists.");
        }
    }
}