using Application.Exceptions;
using Application.Features.Identity.Roles;
using Application.Features.Identity.Users;
using Application.Features.Masters.Responses;
using Domain.Views;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Common;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security;

namespace Infrastructure.Identity
{
    public class RoleService(
        RoleManager<ApplicationRole> roleManager, 
        UserManager<ApplicationUser> userManager, 
        ApplicationDbContext context, ICurrentUserService currentUserService,
        IMultiTenantContextAccessor<CIMTenantInfo> tenantInfoContextAccessor, IConfiguration configuration) : IRoleService
    {
        CommonMethods commonMethods = new(context, currentUserService, configuration);
        public async Task<string> CreateAsync(CreateRolePermissionRequest request)
        {
            var newRole = new ApplicationRole()
            {
                Name = request.Name,
                Description = request.Description
            };

            var result = await roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to create a role.", GetIdentityResultErrorDescriptions(result));
            }
            var roleId = await roleManager.GetRoleIdAsync(newRole);
            
            UpdateRolePermissionsRequest updateRolePermissionsRequest = new();
            updateRolePermissionsRequest.RoleId = roleId;
            updateRolePermissionsRequest.Permissions = commonMethods.GetFormatScreenPermissionsToRoleClaims(request.Permissions);

            var message = await this.UpdatePermissionsAsync(updateRolePermissionsRequest);

            return message; //newRole.Name;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var roleInDb = await roleManager.FindByIdAsync(id)
               ?? throw new NotFoundException("Role does not exists.");
            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictException($"Not allowed to delete '{roleInDb.Name}' role.");
            }

            if ((await userManager.GetUsersInRoleAsync(roleInDb.Name)).Count > 0)
            {
                throw new ConflictException($"Not allowed to delete '{roleInDb.Name}' role as is currently assigned to users.");
            }

            var result = await roleManager.DeleteAsync(roleInDb);

            if (!result.Succeeded)
            {
                throw new IdentityException($"Failed to delete {roleInDb.Name} role.", GetIdentityResultErrorDescriptions(result));
            }

            return roleInDb.Name;
        }

        public async Task<bool> DoesItExistsAsync(string name)
        {
            return await roleManager.RoleExistsAsync(name);
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string id, CancellationToken ct)
        {
            var roleInDb = await context
                .Roles
                .SingleOrDefaultAsync(r => r.Id == id, ct)
                ?? throw new NotFoundException("Role does not exists.");

            var role = roleInDb.Adapt<RoleDto>();
            role.Permissions = await context.RoleClaims
                .Where(rc => rc.RoleId == id && rc.ClaimType == ClaimConstants.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(ct);

            var screenPermissions = commonMethods.GetFormatRoleClaimsToScreenPermissions(role.Permissions);

            RoleResponse roleResponse = new RoleResponse();
            roleResponse.Id = Guid.Parse(role.Id);
            roleResponse.Name = role.Name;
            roleResponse.Description = role.Description;
            roleResponse.Permissions = screenPermissions;

            return roleResponse;
        }

        public async Task<RoleDto> GetRoleEntityIdAsync(string id, CancellationToken ct)
        {
            var roleInDb = await context
                .Roles
                .SingleOrDefaultAsync(r => r.Id == id, ct)
                ?? throw new NotFoundException("Role does not exists.");

            var role = roleInDb.Adapt<RoleDto>();
            role.Permissions = await context.RoleClaims
                .Where(rc => rc.RoleId == id && rc.ClaimType == ClaimConstants.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(ct);         

            return role;
        }

        public async Task<List<string>> GetScreenPermissionByRoleIdAsync(string id, string screenName)
        {
            string strClaimValue = "Permission." + screenName + ".";
            List<string> Permissions = await context.RoleClaims
                .Where(rc => rc.RoleId == id && rc.ClaimType == ClaimConstants.Permission && rc.ClaimValue.StartsWith(strClaimValue))
                .Select(rc => rc.ClaimValue)
                .ToListAsync();

            return Permissions;
        }
        

        public async Task<List<RoleDto>> GetRolesAsync(CancellationToken ct)
        {
            var rolesInDb = await roleManager
                .Roles
                .ToListAsync(ct);

            return rolesInDb.Adapt<List<RoleDto>>();
        }

        //public async Task<RoleResponse> GetRoleWithPermissionsAsync(string id, CancellationToken ct)
        //{
        //    var roleDto = await GetRoleByIdAsync(id, ct);
        //    roleDto.Permissions = await context.RoleClaims
        //        .Where(rc => rc.RoleId ==  id && rc.ClaimType == ClaimConstants.Permission)
        //        .Select(rc => rc.ClaimValue)
        //        .ToListAsync(ct);

        //    return roleDto;
        //}

        public async Task<string> UpdateAsync(UpdateRoleRequest request)
        {
            var roleInDb = await roleManager.FindByIdAsync(request.Id)
                ?? throw new NotFoundException("Role does not exists.");

            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictException($"Changes not allowed on {roleInDb.Name} role.");
            }

            roleInDb.Name = request.Name;
            roleInDb.Description = request.Description;
            roleInDb.NormalizedName = request.Name.ToUpperInvariant();

            var result = await roleManager.UpdateAsync(roleInDb);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to update role.", GetIdentityResultErrorDescriptions(result));
            }

            UpdateRolePermissionsRequest updateRolePermissionsRequest = new();
            updateRolePermissionsRequest.RoleId = roleInDb.Id;
            updateRolePermissionsRequest.Permissions = commonMethods.GetFormatScreenPermissionsToRoleClaims(request.Permissions);

            var message = await this.UpdatePermissionsAsync(updateRolePermissionsRequest);

            return roleInDb.Name;
        }

        public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            var roleInDb = await roleManager.FindByIdAsync(request.RoleId)
                ?? throw new NotFoundException("Role does not exists.");

            if (roleInDb.Name == RoleConstants.Admin)
            {
                throw new ConflictException($"Not allowed to change permissions for {roleInDb.Name} role.");
            }

            if (tenantInfoContextAccessor.MultiTenantContext.TenantInfo.Id != TenancyConstants.Root.Id)
            {
                request.Permissions.RemoveAll(p => p.StartsWith("Permission.Root."));
            }

            var currentClaims = await roleManager.GetClaimsAsync(roleInDb);


            // Remove previously assigned permissions and not current selected as per incoming request.
            foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
            {
                var result = await roleManager.RemoveClaimAsync(roleInDb, claim);

                if (!result.Succeeded)
                {
                    throw new IdentityException("Failed to remove permission.", GetIdentityResultErrorDescriptions(result));
                }
            }

            // Assign newly selected permissions
            foreach (var permission in request.Permissions.Where(p => !currentClaims.Any(c => c.Value == p)))
            {
                await context
                    .RoleClaims
                    .AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = roleInDb.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = permission
                    });
                await context.SaveChangesAsync();
            }

            // adding base view to get master data on all screens
            if (!context.RoleClaims.Any(x => x.ClaimValue.Contains("Base.View") && x.RoleId == roleInDb.Id))
            {
                await context
                        .RoleClaims
                        .AddAsync(new IdentityRoleClaim<string>
                        {
                            RoleId = roleInDb.Id,
                            ClaimType = ClaimConstants.Permission,
                            ClaimValue = "Permission.Base.View"
                        });
                await context.SaveChangesAsync();
            }
            return "Permissions Updated Successfully";
        }

        public async Task<List<ScreenPermissions>> GetAllScreensAsync()
        {
            var screenPermissions = new List<ScreenPermissions>();
            CommonMethods commonMethods = new CommonMethods(context, currentUserService, configuration);
            foreach (VW_ListItems listItem in await context.VW_ListItems.Where(x => x.ListCode == "SCRNS").ToListAsync())
            {
                ScreenPermissions screenPermission = new();
                screenPermission.ScreenId = listItem.ListTypeItemId.ToString();
                screenPermission.ScreenCode = listItem.ItemCode;
                screenPermission.ScreenName = listItem.ItemName;

                screenPermissions.Add(commonMethods.getCategory(screenPermission));
            }
            return screenPermissions;
        }

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }

            return errorDescriptions;
        }        
    }
}
