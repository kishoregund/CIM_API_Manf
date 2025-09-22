namespace Domain.Entities
{
    public class SRPEngWorkTime : BaseEntity
    {
        public DateTime WorkTimeDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PerDayHrs { get; set; }
        public string TotalDays { get; set; }
        public string TotalHrs { get; set; }
        public Guid ServiceReportId { get; set; }
    }
}
