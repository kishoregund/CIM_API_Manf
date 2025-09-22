namespace Application.Features.Customers.Responses
{
    public class SiteResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid CustomerId { get; set; }
        public string RegName { get; set; }
        public string CustRegName { get; set; }
        public string PayTerms { get; set; }
        public bool IsBlocked { get; set; }
        public Guid DistId { get; set; }
        public Guid RegionId { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid CountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        public List<SiteContact> SiteContacts { get; set; }
    }
}
