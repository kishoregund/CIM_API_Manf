using Application.Features.Instruments.Requests;

namespace Application.Features.Instruments.Responses
{
    public class InstrumentAllocationResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
        public Guid InstrumentId { get; set; }
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string Instrument { get; set; }
        public string BusinessUnit { get; set; }
        public string BrandName { get; set; }

    }
}
