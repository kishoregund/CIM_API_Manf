namespace Domain.Entities
{
    public class AMCInstrument : BaseEntity
    {
        public string SerialNos { get; set; }
        public string InsVersion { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public Guid InsTypeId { get; set; }
        public Guid AMCId { get; set; }
        public Guid InstrumentId { get; set; }
    }
}
