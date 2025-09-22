using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetEngSchedulerQuery : IRequest<IResponseWrapper>
    {
        public Guid EngSchedulerId { get; set; }
    }

    public class GetEngSchedulerQueryHandler(IEngSchedulerService EngSchedulerService) : IRequestHandler<GetEngSchedulerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngSchedulerQuery request, CancellationToken cancellationToken)
        {
            var engSchedulerInDb = (await EngSchedulerService.GetEngSchedulerAsync(request.EngSchedulerId)).Adapt<EngSchedulerResponse>();

            if (engSchedulerInDb is not null)
            {
                return await ResponseWrapper<EngSchedulerResponse>.SuccessAsync(data: engSchedulerInDb);
            }
            return await ResponseWrapper<EngSchedulerResponse>.SuccessAsync(message: "EngScheduler does not exists.");
        }
    }
}
