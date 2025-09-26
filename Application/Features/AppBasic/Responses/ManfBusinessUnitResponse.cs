namespace Application.Features.AppBasic.Responses
{
    public class ManfBusinessUnitResponse
    {
        public Guid Id { get; set; }
        public Guid ManfId { get; set; }
        public string BusinessUnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
