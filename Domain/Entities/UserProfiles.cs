namespace Domain.Entities
{
    public class UserProfiles : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string BrandIds { get; set; }
        public Guid SegmentId { get; set; }
        public Guid ProfileFor { get; set; }
        public string BusinessUnitIds { get; set; }
        public string DistRegions { get; set; }
        public string CustSites { get; set; }
        public string ManfSalesRegions { get; set; }
        public string Description { get; set; }
        public bool IsManfSubscribed { get; set; }
    }
}
