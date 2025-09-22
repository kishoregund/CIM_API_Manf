using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users
{
    public class UserTenantDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string TenantId { get; set; }
    }
}
