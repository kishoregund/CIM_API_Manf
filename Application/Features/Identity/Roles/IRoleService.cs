namespace Application.Features.Identity.Roles
{
    public interface IRoleService
    {
        Task<string> CreateAsync(CreateRolePermissionRequest request);
        Task<string> UpdateAsync(UpdateRoleRequest request);
        Task<string> DeleteAsync(string id);
        Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request);
        Task<List<RoleDto>> GetRolesAsync(CancellationToken ct);
        Task<RoleResponse> GetRoleByIdAsync(string id, CancellationToken ct);
        Task<RoleDto> GetRoleEntityIdAsync(string id, CancellationToken ct);        
        Task<List<ScreenPermissions>> GetAllScreensAsync();
        Task<List<string>> GetScreenPermissionByRoleIdAsync(string id, string screenName);

        //Task<RoleDto> GetRoleWithPermissionsAsync(string id, CancellationToken ct);
        Task<bool> DoesItExistsAsync(string name);
    }
}
