using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Requests
{
    public class SparepartOfferRequestRequest
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string PartNo { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AfterDiscount { get; set; }
        public string HsCode { get; set; }
        public Guid SparePartId { get; set; }
        public Guid OfferRequestId { get; set; }
        public Guid CountryId { get; set; }
        public Guid CurrencyId { get; set; }
    }
}
