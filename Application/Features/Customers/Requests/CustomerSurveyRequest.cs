using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Requests
{
    public class CustomerSurveyRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid DistId { get; set; }
        public Guid EngineerId { get; set; }
        public string EngineerName { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string ServiceRequestNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OnTime { get; set; }
        public string IsProfessional { get; set; }
        public string IsNotified { get; set; }
        public string IsSatisfied { get; set; }
        public string IsAreaClean { get; set; }
        public string IsNote { get; set; }
        public string Comments { get; set; }
    }
}
