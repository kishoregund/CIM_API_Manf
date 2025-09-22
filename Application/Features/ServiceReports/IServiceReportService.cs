
using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceReports.Responses;
using Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceReports
{
    public interface IServiceReportService
    {
        Task<Domain.Entities.ServiceReport> GetServiceReportEntityAsync(Guid id);
        Task<ServiceReportResponse> GetServiceReportAsync(Guid id);
        Task<VW_ServiceReport> GetServiceReportPDFAsync(Guid id);        
        Task<List<ServiceReportResponse>> GetServiceReportsAsync();
        Task<Guid> CreateServiceReportAsync(Domain.Entities.ServiceReport ServiceReport);
        Task<Guid> UpdateServiceReportAsync(Domain.Entities.ServiceReport ServiceReport);
        Task<bool> DeleteServiceReportAsync(Guid id);
        Task<bool> UploadServiceReportAsync(UploadServiceReportRequest uploadServiceReport);
    }
}