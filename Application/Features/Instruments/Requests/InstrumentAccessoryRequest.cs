namespace Application.Features.Instruments.Requests
{
    public class InstrumentAccessoryRequest
    {
        public Guid Id { get; set; }        
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
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
