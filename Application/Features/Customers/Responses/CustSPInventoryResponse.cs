using Application.Features.Spares.Responses;

namespace Application.Features.Customers.Responses
{
    public class CustSPInventoryResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int QtyAvailable { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SparePartId { get; set; }
        public Guid SiteId { get; set; }
        public Guid InstrumentId { get; set; }        
        public SparepartResponse SparePart { get; set; }
        public string PartNo { get; set; }
        public string HscCode { get; set; }
        public string PartNoDesc { get; set; }
        public string CustomerName { get; set; }
        public string InstrumentName { get; set; }
    }
}
