using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScreenPermissions> Permissions { get; set; }
    }

    //public class ScreenPermissionsResponse
    //{
    //    public string ScreenId { get; set; }
    //    public string ScreenCode { get; set; }
    //    public string ScreenName { get; set; }
    //    public bool Create { get; set; }
    //    public bool Read { get; set; }
    //    public bool Commercial { get; set; }
    //    public bool Update { get; set; }
    //    public bool Delete { get; set; }
    //    public string Category { get; set; }
    //    public string CategoryName { get; set; }
    //    public string Privilages { get; set; }

    //}
}
