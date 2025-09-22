using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Travels.Requests
{
    public class TravelInvoiceRequest
    {
        public Guid EngineerId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid DistributorId { get; set; }
        public decimal AmountBuild { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }

    public class UpdateTravelInvoiceRequest
    {
        public Guid Id { get; set; }
        public Guid EngineerId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid DistributorId { get; set; }
        public decimal AmountBuild { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
