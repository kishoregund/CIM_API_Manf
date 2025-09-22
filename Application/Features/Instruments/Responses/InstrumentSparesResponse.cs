namespace Application.Features.Instruments.Responses
{
    public class InstrumentSparesResponse
    {
        public Guid Id { get; set; }     
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid InstrumentId { get; set; }
        public Guid ConfigTypeId { get; set; }
        public Guid ConfigValueId { get; set; }
        public string ConfigTypeName { get; set; }
        public string ConfigValueName { get; set; }
        public string PartNo { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public int InsQty { get; set; }
        public Guid PartType { get; set; }
        public string PartTypeName { get; set; }
        public string DescCatalogue { get; set; }
        public string HsCode { get; set; }
        public Guid CountryId { get; set; }
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public string Image { get; set; }
        public bool IsObselete { get; set; }
        public Guid? ReplacePartNoId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string PartNoDesc { get; set; }
    }
}
