using Application.Features.AppBasic.Responses;

namespace Application.Features.AppBasic.Queries
{
    public class GetBrandsQuery : IRequest<IResponseWrapper>
    {
        
    }

    public class GetBrandsQueryHandler(IBrandService brandService) : IRequestHandler<GetBrandsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandsInDb = await brandService.GetBrandsAsync();

            if (brandsInDb.Count > 0)
            {
                return await ResponseWrapper<List<BrandResponse>>.SuccessAsync(data: brandsInDb.Adapt<List<BrandResponse>>());
            }
            return await ResponseWrapper<List<BrandResponse>>.SuccessAsync(message: "No Brands were found.");
        }
    }
}