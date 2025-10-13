namespace Application.Features.AppBasic.Responses
{
    public class BusinessUnitResponse
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string BusinessUnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
