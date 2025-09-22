using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBrandsByBusinessUnitIdQuery : IRequest<IResponseWrapper>
    {
        public Guid BusinessUnitId { get; set; }
    }
    public class GetBrandsByBusinessUnitIdQueryHandler(IBrandService brandService) : IRequestHandler<GetBrandsByBusinessUnitIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBrandsByBusinessUnitIdQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = (await brandService.GetBrandsByBusinessUnitAsync(request.BusinessUnitId)).Adapt<List<BrandResponse>>();

            if (brandInDb is not null)
            {
                return await ResponseWrapper<List<BrandResponse>>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<BrandResponse>.SuccessAsync(message: "Brand does not exists.");
        }
    }
}