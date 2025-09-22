using Application.Features.Notifications;
using Application.Features.Notifications.Requests;
using Domain.Entities;

namespace Application.Features.Notifications.Commands
{
    public class CreateNotificationsCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public NotificationsRequest NotificationsRequest { get; set; }
    }

    public class CreateNotificationsCommandHandler(INotificationsService NotificationsService) : IRequestHandler<CreateNotificationsCommand, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(CreateNotificationsCommand request, CancellationToken cancellationToken)
        {
            // map

            var newNotifications = request.NotificationsRequest.Adapt<Domain.Entities.Notifications>();

            var NotificationsId = await NotificationsService.CreateNotificationsAsync(newNotifications);

            return await ResponseWrapper<Guid>.SuccessAsync(data: NotificationsId, message: "Record saved successfully.");
        }
    }
}
