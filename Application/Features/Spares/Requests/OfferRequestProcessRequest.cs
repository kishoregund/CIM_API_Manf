using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Spares.Requests
{
    public class OfferRequestProcessRequest
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public Guid OfferRequestId { get; set; }
        public Guid Stage { get; set; }
        public int? Index { get; set; }
        public int StageIndex { get; set; }
        public bool IsCompleted { get; set; }
        [SkipGlobalValidation]
        public string PaymentTypeId { get; set; }  // it can be null and multiple
        public decimal PayAmt { get; set; }
        public decimal BaseCurrencyAmt { get; set; }
        public Guid PayAmtCurrencyId { get; set; }
    }
}
