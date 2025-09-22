using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests.Requests
{
    public class ServiceRequestRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public bool IsReportGenerated { get; set; }
        public Guid SiteId { get; set; }
        public Guid CustId { get; set; }
        public Guid DistId { get; set; }
        public string Distributor { get; set; }
        [SkipGlobalValidation]
        public string SerReqNo { get; set; }
        public string SerReqDate { get; set; }
        public string SerResolutionDate { get; set; }
        [SkipGlobalValidation]
        public string CompanyName { get; set; }
        [SkipGlobalValidation]
        public string SiteName { get; set; }
        [SkipGlobalValidation]
        public string SDate { get; set; }
        [SkipGlobalValidation]
        public string EDate { get; set; }
        [SkipGlobalValidation]
        public string ContactPerson { get; set; }
        [SkipGlobalValidation]
        public string OperatorName { get; set; }
        [SkipGlobalValidation]
        public string OperatorEmail { get; set; }
        [SkipGlobalValidation]
        public string MachmodelName { get; set; }
        [SkipGlobalValidation]
        public string XrayGenerator { get; set; }
        [SkipGlobalValidation]
        public string BreakdownType { get; set; }
        [SkipGlobalValidation]
        public string RecurringComments { get; set; }
        public Guid BreakoccurDetailsId { get; set; }
        [SkipGlobalValidation]
        public string ResolveAction { get; set; }
        public string ComplaintRegisName { get; set; }
        public Guid AssignedTo { get; set; }
        public string VisitType { get; set; }
        public string RequestTime { get; set; }
        [SkipGlobalValidation]
        public string MachEngineer { get; set; }
        public string Country { get; set; }
        [SkipGlobalValidation]
        public string Email { get; set; }
        [SkipGlobalValidation]
        public string OperatorNumber { get; set; }
        public string MachinesNo { get; set; }
        public string SampleHandlingType { get; set; }
        public bool IsRecurring { get; set; }
        [SkipGlobalValidation]
        public string AlarmDetails { get; set; }
        [SkipGlobalValidation]
        public string CurrentInstrustatus { get; set; }
        [SkipGlobalValidation]
        public string Escalation { get; set; }
        public string RegistrarPhone { get; set; }
        public bool Accepted { get; set; }
        public string AcceptedDate { get; set; }
        [SkipGlobalValidation]
        public string DelayedReasons { get; set; }
        public bool IsCritical { get; set; }
        [SkipGlobalValidation]
        public string RequestTypeId { get; set; }
        public string SubRequestTypeId { get; set; }
        public Guid StatusId { get; set; }
        public Guid StageId { get; set; }
        [SkipGlobalValidation]
        public Guid SiteUserId { get; set; }
        [SkipGlobalValidation]
        public string Remarks { get; set; }

        public bool IsNotUnderAmc { get; set; }
        public Guid AmcId { get; set; }
        public decimal? CostInUsd { get; set; }
        public Guid? BaseCurrency { get; set; }
        public decimal? BaseAmt { get; set; }
        public decimal? TotalCost { get; set; }
        public string AmcServiceQuote { get; set; }
        public Guid? TotalCostCurrency { get; set; }
    }
}
