namespace Domain.Entities
{
    public class SRAuditTrail:BaseEntity
    {
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Values { get; set; }
        public Guid ServiceRequestId { get; set; }
    }
}
