using Application.Features.Instruments.Responses;

namespace Application.Features.Instruments.Requests
{
    public class InstrumentAllocationRequest
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
    }
}
