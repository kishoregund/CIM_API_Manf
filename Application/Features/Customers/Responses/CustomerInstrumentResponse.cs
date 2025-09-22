namespace Application.Features.Customers.Responses
{
    public class CustomerInstrumentResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid DistId { get; set; }
        public Guid CustSiteId { get; set; }
        public string CustSiteName { get; set; }
        public string DateOfPurchase { get; set; }
        public decimal? Cost { get; set; }
        public decimal? BaseCurrencyAmt { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid InstrumentId { get; set; }
        public string InstrumentSerNoType { get; set; }
        public string InsMfgDt { get; set; }
        public string ShipDt { get; set; }
        public string InstallDt { get; set; }
        public string InstallBy { get; set; }
        public string InstallByName { get; set; }
        public string InstallByOther { get; set; }
        public string EngName { get; set; }
        public string EngNameOther { get; set; }
        public string EngContact { get; set; }
        public string EngEmail { get; set; }
        public Guid OperatorId { get; set; }
        public Guid InstruEngineerId { get; set; }
        public SiteContact OperatorEng { get; set; }
        public SiteContact MachineEng { get; set; }
        public bool Warranty { get; set; }
        public string WrntyStDt { get; set; }
        public string WrntyEnDt { get; set; }
        public Guid ManufId { get; set; }
        public string ManufName { get; set; }
        public string SerialNos { get; set; }
        public string InsType { get; set; }
        public string InsTypeName { get; set; }
        public string InsVersion { get; set; }
    }
}
