
namespace Domain.Entities
{ 
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Iso_2 { get; set; }
        public string Iso_3 { get; set; }
        public string Formal { get; set; }
        public string Sub_Region { get; set; }
        public string Region { get; set; }
        public string Capital { get; set; }
        public Guid ContinentId { get; set; }
        public Guid CurrencyId { get; set; }
    }
}
