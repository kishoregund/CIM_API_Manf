using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class VW_ServiceReport
    {
        public Guid ServiceReportId { get; set; }
        public string Customer { get; set; }
        public string Department { get; set; }
        public string Town { get; set; }
        public string LabChief { get; set; }
        public string Instrument { get; set; }
        public string BrandName { get; set; }
        public string SROF { get; set; }
        public string Country { get; set; }
        public string RespInstrumentFName { get; set; }
        public string RespInstrumentLName { get; set; }
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
        public Guid ServiceRequestId { get; set; }
        public string SerReqNo { get; set; }
        public string RequestType { get; set; }
        public bool Attachment { get; set; }
        public bool IsWorkDone { get; set; }
        public int TotalDays { get; set; }

        public List<SRPEngWorkDone> WorkDone { get; set; }
        public List<SRPEngWorkTime> WorkTime { get; set; }
        public List<SREngComments> EngComments { get; set; }
        public List<SPConsumed> SPConsumed { get; set; }
        public List<SPRecommended> SPRecomm { get; set; }
    }

    public class TotalDays
    {
        public int Totaldays { get; set; }
    }
}
