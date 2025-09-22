namespace Domain.Entities
{
    public class SalesRegion : BaseEntity
    {
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
        public Guid AddrCountryId { get; set; }
        public string Zip { get; set; }
        public decimal GeoLat { get; set; }
        public decimal GeoLong { get; set; }

        #endregion

    }
}