namespace Application.Features.Masters.Responses
{
    public class ConfigTypeValuesResponse
    {
        public Guid Id { get; set; }
        public string ListTypeItemId { get; set; }
        public string ConfigValue { get; set; }        
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
