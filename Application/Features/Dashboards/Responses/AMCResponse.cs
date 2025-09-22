using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class AMCResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BillToName { get; set; }
        public Guid BillTo { get; set; }
        public Guid CustSite { get; set; }
        public string CustSiteName { get; set; }
        public string ServiceQuote { get; set; }
        public string SqDate { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string Project { get; set; }
        public string ServiceType { get; set; }
        public Guid BrandId { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal? Zerorate { get; set; }
        public decimal? BaseCurrencyAmt { get; set; }
        public decimal? ConversionAmount { get; set; }
        public bool IsMultipleBreakdown { get; set; }
        public string TnC { get; set; }
        public string StageName { get; set; }
        public string PaymentTerms { get; set; }
        public string SecondVisitDate { get; set; }
        public string FirstVisitDate { get; set; }
        public string AMCServiceType { get; set; }
        public string AMCServiceTypeCode { get; set; }
        public string InstrumentIds { get; set; }
        public bool IsCompleted { get; set; }

    }
}
