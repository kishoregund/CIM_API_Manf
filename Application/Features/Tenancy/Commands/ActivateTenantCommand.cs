﻿namespace Application.Features.Tenancy.Commands
{
    public class ActivateTenantCommand : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class ActivateTenantCommandHandler(ITenantService tenantService) : IRequestHandler<ActivateTenantCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await tenantService.ActivateAsync(request.TenantId);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, "Tenant activated successfully.");
        }
    }
}
