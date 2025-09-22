using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class ServiceContractRevenueResponse
    {
        public decimal? AmcRevenue { get; set; }
        public decimal? PreventiveRevenue { get; set; }
        public decimal? BreakdownRevenue { get; set; }
        public decimal? OncallRevenue { get; set; }
        public decimal? PlannedRevenue { get; set; }
        public object GrpAmc { get; set; }
    }
}
