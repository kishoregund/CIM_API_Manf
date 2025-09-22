using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class SparesConsumedResponse
    {
        public string SerReqNo { get; set; }
        public string PartNo { get; set; }
        public string QtyConsumed { get; set; }
        public string AssignedTo { get; set; }
        public string BusinessUnitId { get; set; }
        public string BrandId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
