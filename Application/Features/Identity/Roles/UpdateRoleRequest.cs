namespace Application.Features.Identity.Roles
{
    public class UpdateRoleRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScreenPermissions> Permissions { get; set; }
    }  
}
