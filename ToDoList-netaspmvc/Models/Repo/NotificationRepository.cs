using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToDoList_DomainModel.Models;
using ToDoList_netaspmvc.Infrastructure;

namespace ToDoList_netaspmvc.Models.Repo
{
    public class NotificationRepository : INotificationRepository
    {
        private ToDoContext _context { get; }

        public NotificationRepository(ToDoContext context)
        {
            _context = context;
        }
        public bool Create(Notification notification)
        {
            var dateToRemind = DateTime.ParseExact(notification.DateToRemind.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var todayDate = DateTime.Today;
            if (dateToRemind < todayDate)
            {
                notification.IsRead = true;
            }

            _context.Notification.Add(notification);
            int numOfRecs = _context.SaveChanges();

            if (numOfRecs == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Notification> GetAllNotifications()
        {
            return _context.Notification.ToList();
        }

        public List<Notification> GetNotificationsForList(int toDoListId)
        {
            List<Notification> notificationList = _context.Notification.Where(x => x.toDoListId == toDoListId).ToList();
            for (int i = notificationList.Count - 1; i >= 0; i--)
            {
                var notificationDate = DateTime.ParseExact(notificationList[i].DateToRemind.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (DateTime.Today > notificationDate)
                {
                    _context.Notification.Remove(notificationList[i]);
                }
                else if (DateTime.Today != notificationDate)
                {
                    notificationList.RemoveAt(i);
                }
            }

            _context.SaveChanges();
            return notificationList;
        }

        public void UpdateNotification(Notification notification)
        {
            _context.Notification.Update(notification);
        }

        public void DeleteNotificationsForRecord(int recordID)
        {
            List<Notification> notifications = _context.Notification.Where(x => x.recordId == recordID).ToList();

            if (notifications.Count > 0)
            {
                for (int i = 0; i < notifications.Count; i++)
                {
                    _context.Notification.Remove(notifications[i]);
                }
            }

            _context.SaveChanges();
        }

        public void DeleteNotificationsForList(int toDoListId)
        {
            List<Notification> notifications = _context.Notification.Where(x => x.toDoListId == toDoListId).ToList();

            if (notifications.Count > 0)
            {
                for (int i = 0; i < notifications.Count; i++)
                {
                    _context.Notification.Remove(notifications[i]);
                }
            }

            _context.SaveChanges();
        }
    }
}
