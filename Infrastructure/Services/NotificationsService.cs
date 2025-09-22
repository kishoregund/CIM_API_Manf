using Application.Features.Notifications;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Features.Identity.Users;

namespace Infrastructure.Services
{
#pragma warning disable CS9113 // Parameter is unread.
    public class NotificationsService(ApplicationDbContext context, ICurrentUserService currentUserService) : INotificationsService
#pragma warning restore CS9113 // Parameter is unread.
    {
        public async Task<List<Notifications>> GetNotificationsAsync(Guid userId)
            => await context.Notifications.Where(x => x.UserId == userId).ToListAsync();

        public async Task<Notifications> GetNotificationsByIdAsync(Guid id)
            => await context.Notifications.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> CreateNotificationsAsync(Notifications Notifications)
        {
            Notifications.CreatedOn = DateTime.Now;
            Notifications.CreatedBy = Guid.Empty;
            Notifications.UpdatedBy = Guid.Empty;

            await context.Notifications.AddAsync(Notifications);
            await context.SaveChangesAsync();
            return Notifications.Id;
        }

        public async Task<Guid> UpdateNotificationsAsync(Notifications Notifications)
        {
            context.Entry(Notifications).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Notifications.Id;
        }

        public async Task<bool> DeleteNotificationsAsync(Guid id)
        {

            var deleteNotifications = await context
                .Notifications.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (deleteNotifications == null) return true;

            deleteNotifications.IsDeleted = true;
            deleteNotifications.IsActive = false;

            context.Entry(deleteNotifications).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationsByUserAsync(Guid userId)
        {

            var deleteNotifications = await context
                .Notifications.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            if (deleteNotifications == null) return true;

            deleteNotifications.IsDeleted = true;
            deleteNotifications.IsActive = false;

            context.Entry(deleteNotifications).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
