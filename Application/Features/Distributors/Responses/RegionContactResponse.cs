namespace Application.Features.Distributors.Responses
{
    public class RegionContactResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrimaryContactNo { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryContactNo { get; set; }
        public string SecondaryEmail { get; set; }
        public string DesignationId { get; set; }
        public string WhatsappNo { get; set; }
        public bool IsFieldEngineer { get; set; }

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