using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserProfiles.Responses
{
    public class UserByContactResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public Guid ContactId { get; set; }
        public string ContactType { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }

        public string ParentName { get; set; }
        public string ChildName { get; set; }
        public bool IsFieldEngineer { get; set; }

        public Guid DesignationId { get; set; }
        public string Designation { get; set; }
    }
}
