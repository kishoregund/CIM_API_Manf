using Application.Features.Notifications;

namespace Application.Features.Notifications.Commands
{
    public class DeleteNotificationsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid NotificationsId { get; set; }
    }

    public class DeleteNotificationsCommandHandler(INotificationsService NotificationsService) : IRequestHandler<DeleteNotificationsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteNotificationsCommand request, CancellationToken cancellationToken)
        {
            var deletedNotifications = await NotificationsService.DeleteNotificationsAsync(request.NotificationsId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedNotifications, message: "Notifications deleted successfully.");
        }
    }
}
