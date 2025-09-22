using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TravelInvoice : BaseEntity
    {
        public Guid EngineerId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid DistributorId { get; set; }
        public decimal AmountBuild { get; set; }
    }
}
