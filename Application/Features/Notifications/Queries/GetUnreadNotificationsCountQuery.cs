namespace Application.Features.Notifications.Queries
{
    public class GetUnreadNotificationsCountQuery : IRequest<IResponseWrapper>
    {
        public Guid UserId { get; set; }
    }

    public class GetUnreadNotificationsCountQueryHandler(INotificationsService NotificationsService) : IRequestHandler<GetUnreadNotificationsCountQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var unreadCount = await NotificationsService.GetUnreadNotificationsCountAsync(request.UserId);
            return await ResponseWrapper<int>.SuccessAsync(data: unreadCount, message: "Unread notifications count retrieved successfully.");
        }
    }
}
