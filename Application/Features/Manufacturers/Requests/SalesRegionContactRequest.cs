namespace Application.Features.Manufacturers.Requests
{
    public class SalesRegionContactRequest
    {
        public Guid Id { get; set; }
        public Guid SalesRegionId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        [SkipGlobalValidation]
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrimaryContactNo { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryContactNo { get; set; }
        public string SecondaryEmail { get; set; }
        public Guid DesignationId { get; set; }
        public string WhatsappNo { get; set; }

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