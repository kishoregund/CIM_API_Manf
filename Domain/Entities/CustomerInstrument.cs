namespace Domain.Entities
{
    public class CustomerInstrument : BaseEntity
    {
        public Guid CustSiteId { get; set; }
        public Guid InstrumentId { get; set; }
        public string DateOfPurchase { get; set; }
        public decimal? Cost { get; set; }
        public decimal? BaseCurrencyAmt { get; set; }
        public Guid CurrencyId { get; set; }
        public string InsMfgDt { get; set; }        
        public string ShipDt { get; set; }
        public string InstallDt { get; set; }
        public string InstallBy { get; set; }
        public string InstallByOther { get; set; }
        public string EngName { get; set; }
        public string EngNameOther { get; set; }
        public string EngContact { get; set; }
        public string EngEmail { get; set; }
        public Guid OperatorId { get; set; }
        public Guid InstruEngineerId { get; set; }
        public bool Warranty { get; set; }
        public string WrntyStDt { get; set; }
        public string WrntyEnDt { get; set; }
    }
}