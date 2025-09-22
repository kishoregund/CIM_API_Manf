namespace Application.Features.Distributors.Responses
{
    public class RegionResponse
    {
        public Guid Id { get; set; }
        public string Region { get; set; }
        public string DistRegName { get; set; }
        public string PayTerms { get; set; }
        public string PayTermsValue { get; set; }
        public Guid DistId { get; set; }
        public bool IsBlocked { get; set; }
        public string Countries { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsPrincipal { get; set; }
        
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        public List<RegionContact> RegionContacts { get; set; }
    }
}