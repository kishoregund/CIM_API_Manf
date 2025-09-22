namespace Domain.Entities
{
    public class SPConsumed:BaseEntity
    {
        public Guid ConfigType { get; set; }
        public Guid ConfigValue { get; set; }
        public string PartNo { get; set; }
        public string HscCode { get; set; }
        public string ItemDesc { get; set; }
        public string QtyConsumed { get; set; }
        public Guid ServiceReportId { get; set; }
        public string QtyAvailable { get; set; }
        public Guid CustomerSPInventoryId { get; set; }
    }
}
