using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class ServiceRequestRaisedResponse
    {
            public DateTime? Date { get; set; }
            public int Count { get; set; }
            public int TotalCount { get; set; }
            public bool IsDeleted { get; set; }
            public object Object { get; set; }
    }
}
