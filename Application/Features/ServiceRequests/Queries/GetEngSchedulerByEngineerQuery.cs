using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetEngSchedulerByEngineerQuery : IRequest<IResponseWrapper>
    {
        public Guid EngineerId { get; set; }
    }

    public class GetEngSchedulerByEngineerQueryHandler(IEngSchedulerService EngSchedulerService) : IRequestHandler<GetEngSchedulerByEngineerQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngSchedulerByEngineerQuery request, CancellationToken cancellationToken)
        {
            var engSchedulerInDb = (await EngSchedulerService.GetEngSchedulerByEngineerAsync(request.EngineerId)).Adapt<List<EngSchedulerResponse>>();

            if (engSchedulerInDb is not null)
            {
                return await ResponseWrapper<List<EngSchedulerResponse>>.SuccessAsync(data: engSchedulerInDb);
            }
            return await ResponseWrapper<EngSchedulerResponse>.SuccessAsync(message: "Engineer Schedule does not exists.");
        }
    }
}
