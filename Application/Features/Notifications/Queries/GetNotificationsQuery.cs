
using Application.Features.Notifications.Responses;

namespace Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<IResponseWrapper>
    {
        public Guid UserId { get; set; }
    }

    public class GetNotificationsQueryHandler(INotificationsService NotificationsService) : IRequestHandler<GetNotificationsQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var NotificationsInDb = await NotificationsService.GetNotificationsAsync(request.UserId);

            if (NotificationsInDb.Count > 0)
            {
                return await ResponseWrapper<List<NotificationsResponse>>.SuccessAsync(data: NotificationsInDb.Adapt<List<NotificationsResponse>>());
            }
            return await ResponseWrapper<List<NotificationsResponse>>.SuccessAsync(message: "No Notificationss were found.");
        }
    }
}
