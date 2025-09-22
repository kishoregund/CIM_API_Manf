namespace Domain.Entities
{
    public class ServiceRequest :BaseEntity
    {
        public bool IsReportGenerated { get; set; }
        public Guid SiteId { get; set; }
        public Guid CustId { get; set; }
        public Guid DistId { get; set; }
        public string Distributor { get; set; }
        public string SerReqNo { get; set; }
        public string SerReqDate { get; set; }
        public string SerResolutionDate { get; set; }
        public string CompanyName { get; set; }
        public string SiteName { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string ContactPerson { get; set; }
        public string OperatorName { get; set; }
        public string OperatorEmail { get; set; }
        public string MachmodelName { get; set; }
        public string XrayGenerator { get; set; }
        public string BreakdownType { get; set; }
        public string RecurringComments { get; set; }
        public Guid BreakoccurDetailsId { get; set; }
        public string ResolveAction { get; set; }
        public string ComplaintRegisName { get; set; }
        public Guid AssignedTo { get; set; }
        public string VisitType { get; set; }
        public string RequestTime { get; set; }
        public string MachEngineer { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string OperatorNumber { get; set; }
        public string MachinesNo { get; set; }
        public string SampleHandlingType { get; set; }
        public bool IsRecurring { get; set; }
        public string AlarmDetails { get; set; }
        public string CurrentInstrustatus { get; set; }
        public string Escalation { get; set; }
        public string RegistrarPhone { get; set; }
        public bool Accepted { get; set; }
        public string AcceptedDate { get; set; }
        public string DelayedReasons { get; set; }
        public bool IsCritical { get; set; }

        public string RequestTypeId { get; set; }
        public string SubRequestTypeId { get; set; }
        public Guid StatusId { get; set; }
        public Guid StageId { get; set; }
        public Guid? SiteUserId { get; set; }
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
