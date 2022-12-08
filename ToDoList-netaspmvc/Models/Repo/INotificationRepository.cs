using System.Collections.Generic;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface INotificationRepository
    {
        public List<Notification> GetAllNotifications();
        public bool Create(Notification notification);
        public List<Notification> GetNotificationsForList(int toDoListId);
        public bool AreThereNotificationsForList(int toDoListId);
        public void UpdateNotification(Notification notification);
    }
}
