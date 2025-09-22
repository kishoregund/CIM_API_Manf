namespace Application.Features.Customers.Responses
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string CustName { get; set; }
        public string Code { get; set; }
        public string IndustrySegment { get; set; }
        public Guid DefDistRegionId { get; set; }
        public Guid DefDistId { get; set; }
        public string DistributorName { get; set; }
        public string DistributorRegionName { get; set; }
        public Guid CountryId { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        public List<SiteResponse> Sites { get; set; }
    }
}
