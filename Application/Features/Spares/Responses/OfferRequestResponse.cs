using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Responses
{
    public class OfferRequestResponse
    {
        public Guid Id { get; set; }
        public Guid Createdby { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public Guid DistributorId { get; set; }
        public string Distributor { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerSiteId { get; set; }
        public Guid CurrencyId { get; set; }
        public string Status { get; set; }
        public string OtherSpareDesc { get; set; }
        public string StageName { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsShipment { get; set; }
        public string CompletedDate { get; set; }
        public string CompletedComments { get; set; }

        public string PoDate { get; set; }
        public string OffReqNo { get; set; }
        public string SpareQuoteNo { get; set; }
        public string PaymentTerms { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
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
        public Guid LcAdministrativeChargesCurr { get; set; }

    }
}
