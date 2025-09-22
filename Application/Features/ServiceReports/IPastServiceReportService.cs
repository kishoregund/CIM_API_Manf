
using Application.Features.ServiceReports.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface IPastServiceReportService
    {
        Task<PastServiceReport> GetPastServiceReportAsync(Guid id);
        Task<List<PastServiceReportResponse>> GetPastServiceReportsAsync();
        Task<Guid> CreatePastServiceReportAsync(PastServiceReport PastServiceReport);
        Task<Guid> UpdatePastServiceReportAsync(PastServiceReport PastServiceReport);
        Task<bool> DeletePastServiceReportAsync(Guid id);
    }
}