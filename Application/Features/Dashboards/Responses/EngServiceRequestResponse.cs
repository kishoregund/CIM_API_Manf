using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class EngServiceRequestResponse
    {

        public DateTime CreatedOn { get; set; }
        public string ContactId { get; set; }
        public string Createdby { get; set; }
        public string UserName { get; set; }
        public string SerReqNo { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsReportGenerated { get; set; }
        public string MachinesNo { get; set; }
        public string ServiceType { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeId { get; set; }
    }
}
