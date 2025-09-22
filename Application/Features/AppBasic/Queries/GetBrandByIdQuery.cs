using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBrandByIdQuery : IRequest<IResponseWrapper>
    {
        public Guid BrandId { get; set; }
    }

    public class GetBrandByIdQueryHandler(IBrandService brandService) : IRequestHandler<GetBrandByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brandInDb = (await brandService.GetBrandByIdAsync(request.BrandId)).Adapt<BrandResponse>();

            if (brandInDb is not null)
            {
                return await ResponseWrapper<BrandResponse>.SuccessAsync(data: brandInDb);
            }
            return await ResponseWrapper<BrandResponse>.SuccessAsync(message: "Brand does not exists.");
        }
    }
}