namespace Domain.Entities
{
    public class Sparepart : BaseEntity
    {
        public Guid ConfigTypeId { get; set; }
        public Guid ConfigValueId { get; set; }
        public string PartNo { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public Guid PartType { get; set; }
        public string DescCatalogue { get; set; }
        public string HsCode { get; set; }
        public Guid CountryId { get; set; }

        //[Precision(18, 2)]
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        //public string Instrument { get; set; }
        //public string AnalyticalTechnique { get; set; }
        //public string BusinessUnit { get; set; }
        public string Image { get; set; }
        public bool IsObselete { get; set; }
        public Guid? ReplacePartNoId { get; set; }
    }

}
