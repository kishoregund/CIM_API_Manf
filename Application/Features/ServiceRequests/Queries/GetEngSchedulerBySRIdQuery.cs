using Application.Features.ServiceRequests.Responses;

namespace Application.Features.ServiceRequests.Queries
{
    public class GetEngSchedulerBySRIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ServiceRequestId { get; set; }
    }

    public class GetEngSchedulerBySRIdQueryHandler(IEngSchedulerService EngSchedulerBySRIdervice) : IRequestHandler<GetEngSchedulerBySRIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetEngSchedulerBySRIdQuery request, CancellationToken cancellationToken)
        {
            var EngSchedulerBySRIdInDb = await EngSchedulerBySRIdervice.GetEngSchedulerBySRIdAsync(request.ServiceRequestId);

            if (EngSchedulerBySRIdInDb.Count > 0)
            {
                return await ResponseWrapper<List<EngSchedulerResponse>>.SuccessAsync(data: EngSchedulerBySRIdInDb.Adapt<List<EngSchedulerResponse>>());
            }
            return await ResponseWrapper<List<EngSchedulerResponse>>.SuccessAsync(message: "No EngSchedules were found.");
        }
    }
}
