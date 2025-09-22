using Application.Features.Distributors.Responses;

namespace Application.Features.Distributors.Queries
{
    public class GetRegionContactByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid RegionContactId { get; set; }
    }


    public class GetRegionContactByIdQueryHandler(IRegionContactService regionContactService) : IRequestHandler<GetRegionContactByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRegionContactByIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await regionContactService.GetRegionContactAsync(request.RegionContactId)).Adapt<RegionContactResponse>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<RegionContactResponse>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<RegionContactResponse>.SuccessAsync(message: "Region Contact does not exists.");
        }
    }
}