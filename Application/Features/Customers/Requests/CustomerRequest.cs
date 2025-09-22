namespace Application.Features.Customers.Requests
{
    public class CustomerRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string CustName { get; set; }
        [SkipGlobalValidationAttribute]
        public string Code { get; set; }
        public string IndustrySegment { get; set; }
        public Guid DefDistRegionId { get; set; }
        public Guid DefDistId { get; set; }
        public Guid CountryId { get; set; }
        public string Street { get; set; }
        [SkipGlobalValidationAttribute]
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryid { get; set; }
        public string Zip { get; set; }
        public decimal GeoLat { get; set; }
        public decimal GeoLong { get; set; }
    }
}
