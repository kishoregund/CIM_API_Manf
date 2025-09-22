namespace Application.Features.AppBasic.Responses
{
    public class CompanyResponse
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string SecondaryCompanyEmail { get; set; }
        public string CompanyEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}