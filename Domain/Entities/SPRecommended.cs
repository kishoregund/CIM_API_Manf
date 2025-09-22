namespace Domain.Entities
{
    public class SPRecommended:BaseEntity
    {
        public Guid ConfigType { get; set; }
        public Guid ConfigValue { get; set; }
        public string PartNo { get; set; }
        public string HscCode { get; set; }
        public string ItemDesc { get; set; }
        public string QtyRecommended { get; set; }
        public Guid ServiceReportId { get; set; }


     
    }
}
