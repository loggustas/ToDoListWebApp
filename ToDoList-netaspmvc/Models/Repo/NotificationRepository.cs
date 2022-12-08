using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            return _context.Notification.Where(x => x.toDoListId == toDoListId).ToList();
        }

        public void UpdateNotification(Notification notification)
        {
            _context.Notification.Update(notification);
        }

        public bool AreThereNotificationsForList(int toDoListId)
        {
            if(this.GetNotificationsForList(toDoListId).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
