using Application.Features.Identity.Users.Models;
using Domain.Views;

namespace Application.Features.Identity.Users
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(CreateUserRequest request);
        Task<string> UpdateUserAsync(UpdateUserRequest request);
        Task<string> DeleteUserAsync(string userId);
        Task<string> ActivateOrDeactivateAsync(string contactType, bool activation, Guid contactId);
        Task<bool> DeleteContactUserAsync(string contactType, bool activation, Guid contactId);
        Task<string> ChangePasswordAsync(ChangePasswordRequest request);
        Task<string> AssignRolesAsync(string userId, UserRolesRequest request);
        Task<List<UserDto>> GetUsersAsync(CancellationToken ct);
        Task<UserDto> GetUserByIdAsync(string userId, CancellationToken ct);
        Task<UserDto> GetUserByContactIdAsync(Guid contactId, string contactType, CancellationToken ct);
        Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken ct);
        Task<bool> IsEmailTakenAsync(string email);
        Task<List<string>> GetPermissionsAsync(string userId, CancellationToken ct);
        Task<bool> IsPermissionAssigedAsync(string userId, string permission, CancellationToken ct = default);
        Task<string> CreateTenantUserAsync(UserTenantDto cimTenantUserInfo);
        Task<UserDetailsDto> GetUserDetailsAsync(string email, CancellationToken ct);
        Task<VW_UserProfile> GetLoggedInUserDetailsAsync(string email, CancellationToken ct);
        Task<List<string>> GetUserRegionsAsync();

        //Task<UserTenantDto> GetUserTenantAsync(string email);
    }
}
