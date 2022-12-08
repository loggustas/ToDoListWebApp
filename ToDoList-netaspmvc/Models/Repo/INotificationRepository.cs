using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface INotificationRepository
    {
        public List<Notification> GetAllNotifications();
        public bool Create(Notification notification);
        public List<Notification> GetNotificationsForList(int toDoListId);
        public void UpdateNotification(Notification notification);
        public void DeleteNotificationsForRecord(int recordID);
        public void DeleteNotificationsForList(int toDoListId);
    }
}
