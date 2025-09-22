using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class DashboardDataResponse
    {
        public string CustId { get; set; }
        public string CustName { get; set; }
        public string EngName { get; set; }
        public string EngId { get; set; }
        public string DistId { get; set; }
        public string SiteRegion { get; set; }
        public DateTime Createdon { get; set; }

    }
}
