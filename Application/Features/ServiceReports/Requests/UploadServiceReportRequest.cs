using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports.Requests
{
    public class UploadServiceReportRequest
    {
        [SkipGlobalValidation]
        public string Pdf { get; set; }
        public string Email { get; set; }
        public string CustName { get; set; }
        public string SerReqNo { get; set; }
        public Guid SerReqId { get; set; }
        public string EngName { get; set; }
        public string OperatorEmail { get; set; }
    }
}
