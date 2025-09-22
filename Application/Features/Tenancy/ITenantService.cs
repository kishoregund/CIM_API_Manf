﻿using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy
{
    public interface ITenantService
    {
        Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct);
        Task<string> ActivateAsync(string id);
        Task<string> DeactivateAsync(string id);
        Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate);
        Task<List<TenantDto>> GetTenantsAsync();
        Task<TenantDto> GetTenantByIdAsync(string id);        
    }
}
