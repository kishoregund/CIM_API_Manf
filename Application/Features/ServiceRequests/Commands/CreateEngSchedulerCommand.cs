
using Application.Features.ServiceRequests.Requests;

namespace Application.Features.ServiceRequests.Commands
{
    public class CreateEngSchedulerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public EngSchedulerRequest EngSchedulerRequest { get; set; }
    }

    public class CreateEngSchedulerCommandHandler(IEngSchedulerService EngSchedulerService) : IRequestHandler<CreateEngSchedulerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateEngSchedulerCommand request, CancellationToken cancellationToken)
        {
            EngScheduler EngSchedulerInDb = new();
            EngSchedulerInDb.ActionId = request.EngSchedulerRequest.ActionId;
            EngSchedulerInDb.Desc = request.EngSchedulerRequest.Description;
            EngSchedulerInDb.EndTime = request.EngSchedulerRequest.EndTime;
            EngSchedulerInDb.SerReqId = request.EngSchedulerRequest.SerReqId;
            EngSchedulerInDb.EndTimezone = request.EngSchedulerRequest.EndTimezone;
            EngSchedulerInDb.EngId = request.EngSchedulerRequest.EngId;
            EngSchedulerInDb.IsAllDay = request.EngSchedulerRequest.IsAllDay;
            EngSchedulerInDb.IsBlock = request.EngSchedulerRequest.IsBlock;
            EngSchedulerInDb.IsReadOnly = request.EngSchedulerRequest.IsReadOnly;
            EngSchedulerInDb.Location = request.EngSchedulerRequest.Location;
            EngSchedulerInDb.RecurrenceException = request.EngSchedulerRequest.RecurrenceException;
            EngSchedulerInDb.RecurrenceRule = request.EngSchedulerRequest.RecurrenceRule;
            EngSchedulerInDb.ResourceId = request.EngSchedulerRequest.ResourceId;
            EngSchedulerInDb.RoomId = request.EngSchedulerRequest.RoomId;
            EngSchedulerInDb.StartTime = request.EngSchedulerRequest.StartTime;
            EngSchedulerInDb.StartTimezone = request.EngSchedulerRequest.StartTimezone;
            EngSchedulerInDb.Subject = request.EngSchedulerRequest.Subject;
            EngSchedulerInDb.UpdatedBy = request.EngSchedulerRequest.UpdatedBy;

            var EngSchedulerId = await EngSchedulerService.CreateEngSchedulerAsync(EngSchedulerInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: EngSchedulerId, message: "Record saved successfully.");
        }
    }
}
