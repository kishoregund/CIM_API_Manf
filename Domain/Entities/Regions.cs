namespace Domain.Entities
{
    public class Regions : BaseEntity
    {
        public string Region { get; set; }
        public string DistRegName { get; set; }
        public string PayTerms { get; set; }
        public Guid DistId { get; set; }
        public bool IsBlocked { get; set; }
        public string Countries { get; set; }
        public bool IsPrincipal { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
    }
}