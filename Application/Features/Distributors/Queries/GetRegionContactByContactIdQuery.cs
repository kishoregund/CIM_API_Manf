using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetRegionContactByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; } 
    }


    public class GetRegionContactByContactIdQueryHandler(IRegionContactService regionContactService) : IRequestHandler<GetRegionContactByContactIdQuery, IResponseWrapper>
    {        
        public async Task<IResponseWrapper> Handle(GetRegionContactByContactIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await regionContactService.GetRegionContactByContact(request.ContactId)).Adapt<List<RegionContactResponse>>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<List<RegionContactResponse>>.SuccessAsync(message: "Region Contact does not exists.");
        }
    }
}