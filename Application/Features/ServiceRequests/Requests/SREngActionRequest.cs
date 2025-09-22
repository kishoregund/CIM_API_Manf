namespace Application.Features.ServiceRequests.Requests
{
    public class SREngActionRequest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid EngineerId { get; set; }
        public string Actiontaken { get; set; }
        public string Comments { get; set; }
        public string TeamviewRecording { get; set; }
        public DateTime? ActionDate { get; set; }
        public Guid ServiceRequestId { get; set; }
    }
}
