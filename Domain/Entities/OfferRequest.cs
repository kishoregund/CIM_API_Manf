using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfferRequest : BaseEntity
    {
        public Guid DistributorId { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CustomerSiteId { get; set; }
        public string Status { get; set; }
        public string OffReqNo { get; set; }
        public string OtherSpareDesc { get; set; }
        public string PoDate { get; set; }
        public string SpareQuoteNo { get; set; }
        public string PaymentTerms { get; set; }
        public Guid CustomerId { get; set; }
        public string Instruments { get; set; }


        public decimal AirFreightChargesAmt { get; set; }
        public decimal InspectionChargesAmt { get; set; }
        public decimal LcAdministrativeChargesAmt { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal BasePCurrencyAmt { get; set; }

        public bool IsDistUpdated { get; set; }
        public Guid TotalCurr { get; set; }
        public Guid AirFreightChargesCurr { get; set; }
        public Guid InspectionChargesCurr { get; set; }
        public Guid LcadministrativeChargesCurr { get; set; }

    }
}
