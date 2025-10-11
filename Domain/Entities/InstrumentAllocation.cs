namespace Domain.Entities
{
    public class InstrumentAllocation : BaseEntity
    {
        public Guid InstrumentId { get; set; }
        public Guid DistributorId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
    }
}