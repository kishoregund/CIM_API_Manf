namespace Application.Features.ServiceRequests.Responses
{
    public class SREngCommentsResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public DateTime NextDate { get; set; }
        public string Comments { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid EngineerId { get; set; }
        public string EngineerName { get; set; }
    }
}
