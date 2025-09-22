using Application.Features.Distributors.Responses;
using Application.Features.Masters;

namespace Application.Features.Distributors.Queries
{
    public class GetRegionByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid RegionId { get; set; }
    }


    public class GetRegionByIdQueryHandler(IRegionService regionService, IRegionContactService regionContactService, IListTypeItemsService listTypeItemsService) : IRequestHandler<GetRegionByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var regionInDb = (await regionService.GetRegionAsync(request.RegionId)).Adapt<RegionResponse>();
            regionInDb.PayTermsValue = listTypeItemsService.GetListTypeItemAsync(Guid.Parse(regionInDb.PayTerms)).Result.ItemName;
            regionInDb.RegionContacts = await regionContactService.GetRegionContactsAsync(request.RegionId);

            if (regionInDb is not null)
            {
                return await ResponseWrapper<RegionResponse>.SuccessAsync(data: regionInDb);
            }
            return await ResponseWrapper<RegionResponse>.SuccessAsync(message: "Region does not exists.");
        }
    }

}
