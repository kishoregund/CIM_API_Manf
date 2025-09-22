using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSRPEngWorkDoneBySRPIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceReportId { get; set; }
    }

    public class GetSRPEngWorkDoneBySRPIdQueryHandler(ISRPEngWorkDoneService SRPEngWorkDoneService) : IRequestHandler<GetSRPEngWorkDoneBySRPIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRPEngWorkDoneBySRPIdQuery request, CancellationToken cancellationToken)
        {
            var SRPEngWorkDoneInDb = await SRPEngWorkDoneService.GetSRPEngWorkDoneBySRPIdAsync(request.ServiceReportId);

            if (SRPEngWorkDoneInDb.Count > 0)
            {
                return await ResponseWrapper<List<SRPEngWorkDoneResponse>>.SuccessAsync(data: SRPEngWorkDoneInDb.Adapt<List<SRPEngWorkDoneResponse>>());
            }
            return await ResponseWrapper<List<SRPEngWorkDoneResponse>>.SuccessAsync(message: "No SRPEngWorkDone were found.");
        }
    }
}
