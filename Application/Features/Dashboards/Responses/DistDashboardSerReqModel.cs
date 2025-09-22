using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class DistDashboardSerReqModel
    {
        public int ServiceRequestRaised { get; set; }
        public object InstrumentWithHighestServiceRequest { get; set; }
        public List<EngHandlingSReq> EngHandlingReq { get; set; }

    }

    public class EngHandlingSReq
    {
        public string CustId { get; set; }
        public double CustTotalSReq { get; set; }
        public string CustName { get; set; }
        public string EngName { get; set; }
        public string EngId { get; set; }
        public double EngAssignedToCust { get; set; }
        public double EngCustPercent { get; set; }
    }
}
