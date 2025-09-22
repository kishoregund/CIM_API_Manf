namespace Domain.Entities
{
    public class AMC : BaseEntity
    {
        public Guid BillTo { get; set; }
        public Guid CustSite { get; set; }
        public string ServiceQuote { get; set; }
        public string SqDate { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string Project { get; set; }
        public string ServiceType { get; set; }
        public string SecondVisitDate { get; set; }
        public string FirstVisitDate { get; set; }
        public Guid BrandId { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal? Zerorate { get; set; }
        public decimal? BaseCurrencyAmt { get; set; }
        public decimal? ConversionAmount { get; set; }
        public bool IsMultipleBreakdown { get; set; }
        public string TnC { get; set; }
        public string PaymentTerms { get; set; }
    }
}
