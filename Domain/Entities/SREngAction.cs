namespace Domain.Entities
{
    public class SREngAction:BaseEntity
    {
        public Guid EngineerId { get; set; }
        public string Actiontaken { get; set; }
        public string Comments { get; set; }
        public string TeamviewRecording { get; set; }
        public DateTime? ActionDate { get; set; }
        public Guid ServiceRequestId { get; set; }
    }
}
