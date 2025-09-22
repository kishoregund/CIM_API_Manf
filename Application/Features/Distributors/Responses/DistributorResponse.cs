namespace Application.Features.Distributors.Responses
{
    public class DistributorResponse
    {
        public Guid Id { get; set; }
        public string DistName { get; set; }
        public string Payterms { get; set; }
        public string PaytermsName { get; set; }
        public bool IsBlocked { get; set; }        
        public string Code { get; set; }
        public string ManufacturerIds { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string AddrCountryId { get; set; }
        public string AddrCountryName { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        public List<RegionResponse> Regions { get; set; }
    }
}
