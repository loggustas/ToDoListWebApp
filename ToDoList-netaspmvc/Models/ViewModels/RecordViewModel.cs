using System.ComponentModel.DataAnnotations;

namespace ToDoList_netaspmvc.Models.ViewModels
{
    public class RecordViewModel
    {
        public RecordViewModel(int id, string toDoList, int number, string title, string description, string dueDate, string status, int toDoListID, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            Id = id;
            this.toDoList = toDoList;
            Number = number;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            this.toDoListID = toDoListID;
            this.hideCompletedAfter = hideCompletedAfter;
            this.showDueTodayAfter = showDueTodayAfter;
        }

        public RecordViewModel(Record record, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            Id = record.Id;
            this.toDoList = record.toDoList;
            Number = record.Number;
            Title = record.Title;
            Description = record.Description;
            DueDate = record.DueDate;
            Status = record.Status;
            this.toDoListID = record.toDoListID;
            this.hideCompletedAfter = hideCompletedAfter;
            this.showDueTodayAfter = showDueTodayAfter;
        }

        public RecordViewModel()
        {

        }

        public int Id { get; set; }

        public string toDoList { get; set; }

        public int Number { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        public string DueDate { get; set; }

        [Required]
        public string Status { get; set; }

        public int toDoListID { get; set; }

        [StringLength(1200)]
        public string Notes { get; set; }

        public bool hideCompletedAfter { get; set; }

        public bool showDueTodayAfter { get; set; }
    }
}
