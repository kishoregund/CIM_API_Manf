namespace Application.Features.Instruments.Requests
{
    public class InstrumentSparesRequest
    {
        public Guid Id { get; set; }        
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid InstrumentId { get; set; }
        public Guid ConfigTypeId { get; set; }
        public Guid ConfigValueId { get; set; }
        public Guid SparepartId { get; set; }
        public int InsQty { get; set; }
    }
}
