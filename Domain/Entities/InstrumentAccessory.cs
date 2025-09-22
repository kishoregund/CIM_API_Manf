namespace Domain.Entities
{
    public class InstrumentAccessory : BaseEntity
    {
        public string AccessoryName { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string YearOfPurchase { get; set; }
        public int Quantity { get; set; }
        public Guid InstrumentId { get; set; }
        public string AccessoryDescription { get; set; }
    }
}
