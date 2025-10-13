namespace Application.Features.AppBasic.Requests
{
    public class BusinessUnitRequest
    {
        public Guid DistributorId { get; set; }
        public string BusinessUnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;

    }

    public class UpdateBusinessUnitRequest
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public string BusinessUnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;

    }
}
