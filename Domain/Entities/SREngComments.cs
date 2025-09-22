namespace Domain.Entities
{
    public class SREngComments:BaseEntity
    {
        public DateTime? Nextdate { get; set; }
        public string Comments { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid EngineerId { get; set; }
    }
}
