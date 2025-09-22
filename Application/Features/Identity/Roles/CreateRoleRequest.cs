namespace Application.Features.Identity.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateRolePermissionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScreenPermissions> Permissions { get; set; }
    }

    public class ScreenPermissions
    {
        public string ScreenId { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Commercial { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string Privilages { get; set; }

    }
}
