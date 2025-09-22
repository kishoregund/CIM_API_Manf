using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Requests
{
    public class NotificationsRequest
    {
        public Guid Id { get; set; }
        public string Remarks { get; set; }
        public Guid RoleId { get; set; }
        public string RaisedBy { get; set; }
        public Guid UserId { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
