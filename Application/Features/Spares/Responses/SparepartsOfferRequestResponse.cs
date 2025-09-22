using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Responses
{
    public class SparepartsOfferRequestResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string PartNo { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AfterDiscount { get; set; }
        public string ItemDescription { get; set; }
        public string HsCode { get; set; }
        public Guid SparePartId { get; set; }
        public Guid OfferRequestId { get; set; }
        public Guid CurrencyId { get; set; }
        public string Currency { get; set; }
        public Guid CountryId { get; set; }
    }
}
