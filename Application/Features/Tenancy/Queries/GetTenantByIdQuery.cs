using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy.Queries
{
    public class GetTenantByIdQuery : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class GetTenantByIdQueryHandler(ITenantService tenantService) : IRequestHandler<GetTenantByIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var tenantInDb = await tenantService.GetTenantByIdAsync(request.TenantId);
            if (tenantInDb is not null)
            {
                return await ResponseWrapper<TenantDto>.SuccessAsync(data: tenantInDb);
            }
            return await ResponseWrapper<TenantDto>.SuccessAsync(message: "Tenant does not exist.");
        }
    }
}
