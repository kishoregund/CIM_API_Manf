namespace Application.Features.Distributors.Requests
{
    public class DistributorRequest
    {
        public Guid Id { get; set; }
        public Guid ManfBusinessUnitId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string DistName { get; set; }
        public string Payterms { get; set; }
        public bool IsBlocked { get; set; }
        [SkipGlobalValidationAttribute]
        public string Code { get; set; }
        public string ManufacturerIds { get; set; }

        #region Address

        public string Street { get; set; }
        [SkipGlobalValidationAttribute]
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public decimal GeoLat { get; set; }
        public decimal GeoLong { get; set; }

        #endregion
    }
}
