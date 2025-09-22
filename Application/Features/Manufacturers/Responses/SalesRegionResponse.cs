namespace Application.Features.Manufacturers.Responses
{
    public class SalesRegionResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string SalesRegionName { get; set; }
        public string PayTerms { get; set; }
        public Guid ManfId { get; set; }
        public bool IsBlocked { get; set; }
        public string Countries { get; set; }
        public bool IsPrincipal { get; set; }


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

        public List<SalesRegionContactResponse> SalesRegionContacts { get; set; }

    }
}