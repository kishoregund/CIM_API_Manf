namespace Application.Features.ServiceReports.Responses
{
    public class ServiceReportResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public Guid ServiceRequestId { get; set; }
        public string ServiceReportNo { get; set; }
        public string SerReqNo { get; set; }
        public string Customer { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }        
        public string Town { get; set; }
        public Guid InstrumentId { get; set; }
        public string Instrument { get; set; }
        public string LabChief { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }


        public string SrOf { get; set; }
        public string Country { get; set; }
        public Guid RespInstrumentId { get; set; }
        public string RespInstrumentName { get; set; }
        public string ComputerArlsn { get; set; }
        public string Software { get; set; }
        public string Firmaware { get; set; }


        public bool Installation { get; set; }
        public bool AnalyticalAssit { get; set; }
        public bool PrevMaintenance { get; set; }
        public bool Rework { get; set; }
        public bool CorrMaintenance { get; set; }
        public string Problem { get; set; }


        public bool WorkFinished { get; set; }
        public bool WorkCompleted { get; set; }
        public bool Interrupted { get; set; }
        public string Reason { get; set; }
        public string NextVisitScheduled { get; set; }
        public string EngineerComments { get; set; }
        public DateTime ServiceReportDate { get; set; }
        public string SignEngName { get; set; }
        public string SignCustName { get; set; }
        public string EngSignature { get; set; }
        public string CustSignature { get; set; }
        public bool IsCompleted { get; set; }
        


        public List<SRPEngWorkTimeResponse> WorkTime { get; set; }
        public List<SRPEngWorkDoneResponse> WorkDone { get; set; }
        public List<SPConsumedResponse> SPConsumed { get; set; }
        public List<SPRecommendedResponse> SPRecommended { get; set; }
        public ServiceRequest ServiceRequest { get; set; }

        
    }
}
