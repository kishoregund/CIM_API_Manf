using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Requests
{
    public class DashboardDateRequest
    {
        public DateTime SDate { get; set; }
        public DateTime EDate { get; set; }
        public DateTime CreatedOn { get; set; }
        [SkipGlobalValidation]
        public string DistId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
    }
}
