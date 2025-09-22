using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications
{
    public interface INotificationsService
    {
        Task<Domain.Entities.Notifications> GetNotificationsByIdAsync(Guid id);
        Task<List<Domain.Entities.Notifications>> GetNotificationsAsync(Guid userId);
        Task<Guid> CreateNotificationsAsync(Domain.Entities.Notifications Notification);
        Task<Guid> UpdateNotificationsAsync(Domain.Entities.Notifications Notification);
        Task<bool> DeleteNotificationsAsync(Guid id);
        Task<bool> DeleteNotificationsByUserAsync(Guid userId);
    }
}
