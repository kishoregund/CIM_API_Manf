using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SparepartsOfferRequest : BaseEntity
    {
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
