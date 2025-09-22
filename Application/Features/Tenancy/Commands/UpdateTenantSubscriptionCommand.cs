using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy.Commands
{
    public class UpdateTenantSubscriptionCommand : IRequest<IResponseWrapper>
    {
        public UpdateTenantSubscriptionRequest TenantRequest { get; set; }
    }

    public class UpdateTenantSubscriptionCommandHandler(ITenantService tenantService) : IRequestHandler<UpdateTenantSubscriptionCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateTenantSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await tenantService
                .UpdateSubscriptionAsync(request.TenantRequest.TenantId, request.TenantRequest.NewExpiryDate);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, "Tenant subscription updated successfully.");
        }
    }
}
