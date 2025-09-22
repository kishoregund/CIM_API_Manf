using Application.Features.Distributors.Responses;

namespace Application.Features.Manufacturers.Responses
{
    public class ManufacturerResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string ManfName { get; set; }
        public string Payterms { get; set; }
        public string PaytermsName { get; set; }
        public bool IsBlocked { get; set; }        
        public string Code { get; set; }

        #region Address

        public string Street { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        #endregion

        public List<SalesRegionResponse> SalesRegions { get; set; }
    }
}
