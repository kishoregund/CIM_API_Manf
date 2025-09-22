using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserProfiles.Requests
{
    public class UserProfilesRequest 
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        [SkipGlobalValidation]
        public string BrandIds { get; set; }
        public Guid SegmentId { get; set; }
        public Guid ProfileFor { get; set; }
        [SkipGlobalValidation]
        public string BusinessUnitIds { get; set; }
        [SkipGlobalValidation]
        public string DistRegions { get; set; }
        [SkipGlobalValidation]
        public string CustSites { get; set; }
        public string Description { get; set; }
    }
}