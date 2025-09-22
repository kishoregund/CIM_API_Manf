namespace Domain.Entities
{
    public class Site: BaseEntity
    {
        public string RegName { get; set; }
        public string CustRegName { get; set; }
        public string PayTerms { get; set; }
        public bool IsBlocked { get; set; }
        public Guid CustomerId { get; set; }
        public Guid DistId { get; set; }
        public Guid RegionId { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid CountryId { get; set; }
        public string Zip { get; set; }
        public decimal GeoLat { get; set; }
        public decimal GeoLong { get; set; }
    }
}
