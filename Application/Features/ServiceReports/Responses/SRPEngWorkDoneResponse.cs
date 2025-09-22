namespace Application.Features.ServiceReports.Responses
{
    public class SRPEngWorkDoneResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string Workdone { get; set; }
        public string Remarks { get; set; }
        public Guid ServiceReportId { get; set; }
    }
}
