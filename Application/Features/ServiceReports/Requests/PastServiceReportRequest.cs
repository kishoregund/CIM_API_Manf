namespace Application.Features.ServiceReports.Requests
{
    public class PastServiceReportRequest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid CustomerId { get; set; }
        public Guid SiteId { get; set; }
        public Guid InstrumentId { get; set; }
        public Guid BrandId { get; set; }
        public string Of { get; set; }
    }
}
