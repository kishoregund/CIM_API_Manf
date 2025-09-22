namespace Application.Features.Customers.Requests
{
    public class CustomerInstrumentRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid CustSiteId { get; set; }
        public Guid InstrumentId { get; set; }
        public string DateOfPurchase { get; set; }
        public decimal? Cost { get; set; }
        public decimal? BaseCurrencyAmt { get; set; }
        public string SerialNos { get; set; }
        public Guid CurrencyId { get; set; }
        public string InsMfgDt { get; set; }
        public string InsType { get; set; }
        public string InsVersion { get; set; }
        public string Image { get; set; }
        public string ShipDt { get; set; }
        public string InstallDt { get; set; }
        public string InstallBy { get; set; }
        [SkipGlobalValidation]
        public string InstallByOther { get; set; }
        [SkipGlobalValidation]
        public string EngName { get; set; }
        [SkipGlobalValidation]
        public string EngNameOther { get; set; }
        public string EngContact { get; set; }
        public string EngEmail { get; set; }
        public Guid OperatorId { get; set; }
        public Guid InstruEngineerId { get; set; }
        public bool Warranty { get; set; }
        [SkipGlobalValidation]
        public string WrntyStDt { get; set; }
        [SkipGlobalValidation]
        public string WrntyEnDt { get; set; }
    }
}
