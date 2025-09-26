namespace Domain.Entities
{
   // [Table("distributor")]
    public class Distributor : BaseEntity
    {
        public Guid ManfBusinessUnitId { get; set; }
        public string DistName { get; set; }
        public string Payterms { get; set; }
        public bool IsBlocked { get; set; }        
        public string Code { get; set; }
        public string ManufacturerIds { get; set; }

        #region Address

        public string Street { get; set; }
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