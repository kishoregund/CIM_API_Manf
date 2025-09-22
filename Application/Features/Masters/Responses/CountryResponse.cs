namespace Application.Features.Masters.Responses
{
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Iso_2 { get; set; }
        public string Iso_3 { get; set; }
        public string Formal { get; set; }
        public string Sub_Region { get; set; }
        public string Region { get; set; }
        public string Capital { get; set; }
        public string ContinentId { get; set; }
        public string CurrencyId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
