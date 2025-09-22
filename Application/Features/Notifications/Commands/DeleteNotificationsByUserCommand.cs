using Application.Features.Notifications;

namespace Application.Features.Notifications.Commands
{
    public class DeleteNotificationsByUserCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public Guid UserId { get; set; }
    }

    public class DeleteNotificationsByUserCommandHandler(INotificationsService NotificationsService) : IRequestHandler<DeleteNotificationsByUserCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(DeleteNotificationsByUserCommand request, CancellationToken cancellationToken)
        {
            var deletedNotifications = await NotificationsService.DeleteNotificationsByUserAsync(request.UserId);

            return await ResponseWrapper<bool>.SuccessAsync(data: deletedNotifications, message: "Notifications deleted successfully.");
        }
    }
}
