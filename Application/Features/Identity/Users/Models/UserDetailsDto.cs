using Application.Features.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Models
{
    public class UserDetailsDto
    {
        public UserDto User { get; set; }
        public string UserRole { get; set; }
        public List<string> Permissions { get; set; }

    }
}
