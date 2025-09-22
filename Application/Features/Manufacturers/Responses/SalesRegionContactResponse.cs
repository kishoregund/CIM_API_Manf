namespace Application.Features.Manufacturers.Responses
{
    public class SalesRegionContactResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
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
        public string Area { get; set; }
        public string Place { get; set; }
        public string City { get; set; }
        public string AddrCountryId { get; set; }
        public string Zip { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }

        #endregion

    }
}