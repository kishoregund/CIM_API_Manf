namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string CustName { get; set; }
        public string Code { get; set; }
        public string IndustrySegment { get; set; }
        public Guid DefDistRegionId { get; set; }
        public Guid DefDistId { get; set; }
        public Guid CountryId { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public decimal GeoLat { get; set; }
        public decimal GeoLong { get; set; }

    }
}
