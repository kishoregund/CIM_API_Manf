namespace Application.Features.Masters.Responses
{
    public class ListTypeItemsResponse
    {
        public Guid Id { get; set; }
        public Guid ListTypeId { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public bool IsEscalationSupervisor { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
