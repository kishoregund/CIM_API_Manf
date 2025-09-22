namespace Application.Features.ServiceReports.Responses
{
    public class SRPEngWorkTimeResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public DateTime WorkTimeDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PerDayHrs { get; set; }
        public string TotalDays { get; set; }
        public string TotalHrs { get; set; }
        public Guid ServiceReportId { get; set; }
    }
}
