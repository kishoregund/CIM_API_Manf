using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfferRequestProcess : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public Guid OfferRequestId { get; set; }
        public Guid Stage { get; set; }
        public int? Index { get; set; }
        public int StageIndex { get; set; }
        public bool IsCompleted { get; set; }
        public string PaymentTypeId { get; set; }
        public decimal PayAmt { get; set; }
        public decimal BaseCurrencyAmt { get; set; }
        public Guid PayAmtCurrencyId { get; set; }
    }
}
