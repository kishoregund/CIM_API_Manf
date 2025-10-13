namespace Domain.Entities
{
    public class BusinessUnit : BaseEntity
    {
        public Guid DistributorId { get; set; }
        public string BusinessUnitName { get; set; }
    }
}
