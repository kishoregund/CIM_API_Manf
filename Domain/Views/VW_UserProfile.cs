using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Views
{    
    public class VW_UserProfile
    {
        public Guid UserProfileId { get; set; }
        public string Description { get; set; }
        public string BrandIds { get; set; }
        public string SelectedBrandId { get; set; }
        public string SelectedBusinessUnitId { get; set; }
        public string BusinessUnitIds { get; set; }
        //public string BusinessUnitName { get; set; }
        //public string BrandName { get; set; }
        public string CustSites { get; set; }
        public string DistRegions { get; set; }
        public string ManfSalesRegions { get; set; }
        public string ManfBUIds { get; set; }
        public bool ActiveUserProfile { get; set; }
        public Guid ProfileFor { get; set; }
        public Guid RoleId { get; set; }
        public Guid SegmentId { get; set; }
        public Guid UserId { get; set; }
        public string SegmentCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool ActiveUser { get; set; }
        public string UserRole { get; set; }
        public Guid ContactId { get; set; }
        public string ContactType { get; set; }
        public Guid EntityChildId { get; set; }
        public string EntityChildName { get; set; }
        public Guid EntityParentId { get; set; }
        public string EntityParentName { get; set; }
        public Guid DesignationId { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public bool IsManfSubscribed { get; set; }
    }
}
