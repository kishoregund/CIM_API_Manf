using Application.Features.ServiceReports.Responses;

namespace Application.Features.ServiceReports.Queries
{
    public class GetSRPEngWorkDoneQuery : IRequest<IResponseWrapper>
    {
        public Guid SRPEngWorkDoneId { get; set; }
    }

    public class GetSRPEngWorkDoneQueryHandler(ISRPEngWorkDoneService SRPEngWorkDoneService) : IRequestHandler<GetSRPEngWorkDoneQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetSRPEngWorkDoneQuery request, CancellationToken cancellationToken)
        {
            var SRPEngWorkDoneInDb = (await SRPEngWorkDoneService.GetSRPEngWorkDoneAsync(request.SRPEngWorkDoneId)).Adapt<SRPEngWorkDoneResponse>();

            if (SRPEngWorkDoneInDb is not null)
            {
                return await ResponseWrapper<SRPEngWorkDoneResponse>.SuccessAsync(data: SRPEngWorkDoneInDb);
            }
            return await ResponseWrapper<SRPEngWorkDoneResponse>.SuccessAsync(message: "SRPEngWorkDone does not exists.");
        }
    }
}
