using System.Collections.Generic;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface INotificationRepository
    {
        public List<Notification> GetAllNotifications();
        public bool Create(Notification notification);
        public Notification GetNotification(int id);
        public void UpdateNotification(Notification notification);
    }
}
