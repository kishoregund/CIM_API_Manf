namespace Application.Features.Distributors.Requests
{
    public class RegionRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string Region { get; set; }
        public string DistRegName { get; set; }
        public string PayTerms { get; set; }
        public Guid DistId { get; set; }
        public bool IsBlocked { get; set; }
        public string Countries { get; set; }
        public bool IsPrincipal { get; set; } = false;

        #region Address

        public string Street { get; set; }
        [SkipGlobalValidationAttribute]
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        #endregion

    }
}