namespace Domain.Entities
{
    public class ListTypeItems : BaseEntity
    {       
        public Guid ListTypeId { get; set; }
        public bool IsEscalationSupervisor { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
    }
}
