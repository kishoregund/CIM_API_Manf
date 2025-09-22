using Application.Features.ServiceRequests.Requests;
using System;

namespace Application.Features.ServiceRequests.Commands
{
    public class UpdateEngSchedulerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public EngSchedulerRequest EngSchedulerRequest { get; set; }
    }

    public class UpdateEngSchedulerCommandHandler(IEngSchedulerService EngSchedulerService) : IRequestHandler<UpdateEngSchedulerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(UpdateEngSchedulerCommand request, CancellationToken cancellationToken)
        {
            var EngSchedulerInDb = await EngSchedulerService.GetEngSchedulerAsync(Guid.Parse(request.EngSchedulerRequest.Id));


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

            var updateEngSchedulerId = await EngSchedulerService.UpdateEngSchedulerAsync(EngSchedulerInDb);

            return await ResponseWrapper<Guid>.SuccessAsync(data: updateEngSchedulerId, message: "Record updated successfully.");
        }
    }
}
