using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AdvanceRequest : BaseEntity
    {
        public Guid EngineerId { get; set; }
        public Guid DistributorId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid CustomerId { get; set; }
        public bool UnderTaking { get; set; } = false;
        public bool IsBillable { get; set; } = false;
        public decimal AdvanceAmount { get; set; }
        public Guid AdvanceCurrency { get; set; }
        public string ReportingManager { get; set; }
        public string ClientNameLocation { get; set; }
        public Guid OfficeLocationId { get; set; }
    }
}
