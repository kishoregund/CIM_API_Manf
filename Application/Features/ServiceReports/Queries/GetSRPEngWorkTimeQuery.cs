using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSRPEngWorkTimeQuery : IRequest<IResponseWrapper>
    {
        public Guid SRPEngWorkTimeId { get; set; }
    }

    public class GetSRPEngWorkTimeQueryHandler(ISRPEngWorkTimeService SRPEngWorkTimeService) : IRequestHandler<GetSRPEngWorkTimeQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRPEngWorkTimeQuery request, CancellationToken cancellationToken)
        {
            var SRPEngWorkTimeInDb = (await SRPEngWorkTimeService.GetSRPEngWorkTimeAsync(request.SRPEngWorkTimeId)).Adapt<SRPEngWorkTimeResponse>();

            if (SRPEngWorkTimeInDb is not null)
            {
                return await ResponseWrapper<SRPEngWorkTimeResponse>.SuccessAsync(data: SRPEngWorkTimeInDb);
            }
            return await ResponseWrapper<SRPEngWorkTimeResponse>.SuccessAsync(message: "SRPEngWorkTime does not exists.");
        }
    }
}
