namespace Domain.Entities
{
    public class InstrumentSpares: BaseEntity
    {
        public Guid InstrumentId { get; set; }
        public Guid ConfigTypeId { get; set; }
        public Guid ConfigValueId { get; set; }
        public Guid SparepartId { get; set; }
        public int InsQty { get; set; }
    }
}
