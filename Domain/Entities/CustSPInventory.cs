namespace Domain.Entities
{
    public class CustSPInventory : BaseEntity
    {
        public int QtyAvailable { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SparePartId { get; set; }
        public Guid SiteId { get; set; }
        public Guid InstrumentId { get; set; }
    }
}
