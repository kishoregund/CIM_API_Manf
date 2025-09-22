using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Responses
{
    public class OfferRequestProcessResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public string Comments { get; set; }
        public Guid OfferRequestId { get; set; }
        public Guid Stage { get; set; }
        public int? Index { get; set; }
        public bool IsCompleted { get; set; }
        public string PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public decimal PayAmt { get; set; }
        public decimal BaseCurrencyAmt { get; set; }
        public string StageName { get; set; }
        public Guid PayAmtCurrencyId { get; set; }
        public string PayAmtCurrency { get; set; }
        public int StageIndex { get; set; }

    }
}
