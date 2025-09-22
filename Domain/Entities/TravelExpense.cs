using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TravelExpense : BaseEntity
    {
        public Guid EngineerId { get; set; }
        public Guid DistributorId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public Guid Designation { get; set; }
        public decimal GrandCompanyTotal { get; set; }
        public decimal GrandEngineerTotal { get; set; }
    }
}
