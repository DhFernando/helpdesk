using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDesk.Entities.Models;

namespace HelpDesk.Entities.Contracts
{
    public interface INotificationRepository : IRepositoryBase<NotificationModel>
    {
        Task<IEnumerable<NotificationModel>> GetNotificationsForUser(string userId);
        Task<NotificationModel> GetNotificationById(Guid id);
        void CreateNotification(string notificationType, string ticketId, string userId);
        void UpdateNotification(NotificationModel notification);
        void DeleteNotification(NotificationModel notification);
    }
}