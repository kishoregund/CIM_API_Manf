namespace Domain.Entities
{
    public class SRPEngWorkDone:BaseEntity
    {
        public string Workdone { get; set; }
        public string Remarks { get; set; }
        public Guid ServiceReportId { get; set; }
    }
}
