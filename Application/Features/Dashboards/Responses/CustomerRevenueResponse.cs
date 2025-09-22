using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class CustomerRevenueResponse
    {
        public decimal? Total { get; set; }
        public Customer Customer { get; set; }
    }
}
