using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class VW_SparepartConsumedHistory
    {
        public Guid CustomerId { get; set; }
        public Guid CustomerSPInventoryId { get; set; }
        public Guid DefDistRegionId { get; set; }
        public bool IsDeleted { get; set; }
        public string QtyConsumed { get; set; }
        public string SerReqNo { get; set; }
        public DateTime ServiceReportDate { get; set; }
    }
}
