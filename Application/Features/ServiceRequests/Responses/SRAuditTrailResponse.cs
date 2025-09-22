namespace Application.Features.ServiceRequests.Responses
{
    public class SRAuditTrailResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Values { get; set; }
        public Guid ServiceRequestId { get; set; }
    }
}
