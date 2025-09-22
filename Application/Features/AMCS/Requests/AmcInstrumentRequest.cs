namespace Application.Features.AMCS.Requests
{
    public class AmcInstrumentRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
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
