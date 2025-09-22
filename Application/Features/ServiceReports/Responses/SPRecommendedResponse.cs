namespace Application.Features.ServiceReports.Responses
{
    public class SPRecommendedResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid ConfigType { get; set; }
        public Guid ConfigValue { get; set; }
        public string PartNo { get; set; }
        public string HscCode { get; set; }
        public string ItemDesc { get; set; }
        public string QtyRecommended { get; set; }
        public string ServiceReportId { get; set; }

    }
}
