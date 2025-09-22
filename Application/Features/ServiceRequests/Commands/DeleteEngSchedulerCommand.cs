

namespace Application.Features.ServiceRequests.Commands
{
    public class DeleteEngSchedulerCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid EngSchedulerId { get; set; }
    }

    public class DeleteEngSchedulerCommandHandler(IEngSchedulerService EngSchedulerService) : IRequestHandler<DeleteEngSchedulerCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteEngSchedulerCommand request, CancellationToken cancellationToken)
        {
            var deletedEngScheduler = await EngSchedulerService.DeleteEngSchedulerAsync(request.EngSchedulerId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedEngScheduler, message: "EngScheduler deleted successfully.");
        }
    }
}
