namespace Domain.Entities
{
    public class EngScheduler: BaseEntity
    {
        public string Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsBlock { get; set; }
        public bool IsReadOnly { get; set; }
        public string RoomId { get; set; }
        public string ResourceId { get; set; }
        public Guid SerReqId { get; set; }
        public Guid ActionId { get; set; }
        public Guid EngId { get; set; }
        public string Location { get; set; }
        public string Desc { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
    }
}
