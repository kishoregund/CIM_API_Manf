using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBrandsByBusinessUnitsQuery : IRequest<IResponseWrapper>
    {
        public string BusinessUnits { get; set; }
    }
    public class GetBrandsByBusinessUnitsQueryHandler(IBrandService brandService) : IRequestHandler<GetBrandsByBusinessUnitsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBrandsByBusinessUnitsQuery request, CancellationToken cancellationToken)
        {
            if (request.BusinessUnits != null)
            {
                var brandInDb = (await brandService.GetBrandsByBusinessUnitsAsync(request.BusinessUnits)).Adapt<List<BrandResponse>>();

                if (brandInDb is not null)
                {
                    return await ResponseWrapper<List<BrandResponse>>.SuccessAsync(data: brandInDb);
                }
            }
            return await ResponseWrapper<BrandResponse>.SuccessAsync(message: "Brand does not exists.");
        }
    }
}