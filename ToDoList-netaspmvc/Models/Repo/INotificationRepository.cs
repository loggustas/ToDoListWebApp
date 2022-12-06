using System.Collections.Generic;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface INotificationRepository
    {
        public List<Notification> GetAllNotifications();
        public bool Create(Notification notification);
        public Notification ReadNotification(int id);
        public void UpdateNotification(Notification notification);
    }
}
