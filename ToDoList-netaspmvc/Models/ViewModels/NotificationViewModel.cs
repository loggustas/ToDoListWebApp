using System.ComponentModel.DataAnnotations;

namespace ToDoList_netaspmvc.Models.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public int toDoListId { get; set; }

        public int recordId { get; set; }

        public string recordTitle { get; set; }

        public string recordDescription { get; set; }

        public string DueDate { get; set; }

        [Required]
        public string DateToRemind { get; set; }

        public bool IsRead { get; set; }

        [StringLength(200)]
        public string Text { get; set; }

        public bool hideCompletedAfter { get; set; }

        public bool showDueTodayAfter { get; set; }

    }
}
