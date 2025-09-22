namespace Application.Features.Masters.Requests
{
    public class CurrencyRequest
    {
        public Guid Id { get; set; }
        public string MCId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int N_Code { get; set; }
        public int Minor_Unit { get; set; }
        [SkipGlobalValidation]
        public string Symbol { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
