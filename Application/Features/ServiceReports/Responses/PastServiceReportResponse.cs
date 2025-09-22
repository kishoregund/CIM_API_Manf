namespace Application.Features.ServiceReports.Responses
{
    public class PastServiceReportResponse
    {
        public Guid Id { get; set; }
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
        public Guid FileId { get; set; }
        public string Of { get; set; }
        public string CustomerName { get; set; }
        public string SiteName { get; set; }
        public string Instrument { get; set; }
        public string BrandName { get; set; }
    }
}
