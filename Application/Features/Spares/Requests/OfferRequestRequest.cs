using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Requests
{
    public class OfferRequestRequest
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid DistributorId { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CustomerSiteId { get; set; }
        [SkipGlobalValidation]
        public string Status { get; set; }
        [SkipGlobalValidation]
        public string OffReqNo { get; set; }
        [SkipGlobalValidation]
        public string OtherSpareDesc { get; set; }
        public string PoDate { get; set; }
        [SkipGlobalValidation]
        public string SpareQuoteNo { get; set; }
        [SkipGlobalValidation]
        public string PaymentTerms { get; set; }
        public Guid CustomerId { get; set; }
        public string Instruments { get; set; }
        public string InstrumentsList { get; set; }


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
