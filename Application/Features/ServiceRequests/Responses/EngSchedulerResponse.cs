namespace Application.Features.ServiceRequests.Responses
{
    public class EngSchedulerResponse
    {
        public Guid Id { get; set; }   
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public string DisplayName { get; set; }
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
        public string EngineerName  { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
    }
}
