namespace Domain.Entities
{
    public class MasterData : BaseEntity
    {       
        public string ListTypeId { get; set; }
        public bool IsEscalationSupervisor { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
    }
}
