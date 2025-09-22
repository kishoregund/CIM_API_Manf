namespace Domain.Entities
{
    public class SRAssignedHistory: BaseEntity
    {
        public Guid EngineerId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string TicketStatus { get; set; }
        public string Comments { get; set; }
        public Guid ServiceRequestId { get; set; }
    }
}
