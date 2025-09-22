namespace Domain.Entities
{
    public class RegionContact : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrimaryContactNo { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryContactNo { get; set; }
        public string SecondaryEmail { get; set; }
        public Guid? DesignationId { get; set; }
        public string WhatsappNo { get; set; }
        public Guid RegionId { get; set; }
        public bool IsFieldEngineer { get; set; } 

        #region Address

        public string Street { get; set; }
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