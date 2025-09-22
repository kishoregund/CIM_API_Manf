namespace Domain.Entities
{
    public class PastServiceReport:BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid SiteId { get; set; }
        public Guid InstrumentId { get; set; }
        public Guid BrandId { get; set; }
        public string Of { get; set; }
    }
}
