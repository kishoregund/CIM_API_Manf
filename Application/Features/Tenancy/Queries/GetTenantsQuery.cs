using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy.Queries
{
    public class GetTenantsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetTenantsQueryHandler(ITenantService tenantService) : IRequestHandler<GetTenantsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenantsInDb = await tenantService.GetTenantsAsync();
            return await ResponseWrapper<List<TenantDto>>.SuccessAsync(data: tenantsInDb);
        }
    }
}
