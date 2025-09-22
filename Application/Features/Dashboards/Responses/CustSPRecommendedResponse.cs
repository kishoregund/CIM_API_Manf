using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class CustSPRecommendedResponse
    {
        public DateTime CreatedOn { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid Createdby { get; set; }
        public string Instrument { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CustId { get; set; }
        public Guid DefDistRegionId { get; set; }
        public Guid ServiceReportId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
        public string SerReqNo { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToFName { get; set; }
        public string AssignedToLName { get; set; }
        public DateTime ServiceReportDate { get; set; }
        public Guid SpareRecomId { get; set; }
        public string PartNo { get; set; }
        public string HscCode { get; set; }
        public string QtyRecommended { get; set; }
        public string DefaultDistributor { get; set; }
        public Guid SiteRegion { get; set; }

    }
}
