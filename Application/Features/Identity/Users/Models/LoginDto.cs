using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Models
{
    public class LoginDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string BusinessUnitId { get; set; }
        public string BrandId { get; set; }
    }
}
