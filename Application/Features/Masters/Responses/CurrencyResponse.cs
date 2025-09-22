namespace Application.Features.Masters.Responses
{
    public class CurrencyResponse
    {
        public Guid Id { get; set; }
        public string McId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int N_Code { get; set; }
        public int Minor_Unit { get; set; }
        public string Symbol { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
