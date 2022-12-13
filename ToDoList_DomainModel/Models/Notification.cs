using ToDoList_DomainModel.ViewModels;

namespace ToDoList_DomainModel.Models
{
    public class Notification
    {
        public Notification(NotificationViewModel viewModel)
        {
            Id = viewModel.Id;
            toDoListId = viewModel.toDoListId;
            recordId = viewModel.recordId;
            recordTitle = viewModel.recordTitle;
            recordDescription = viewModel.recordDescription;
            DueDate = viewModel.DueDate;
            DateToRemind = viewModel.DateToRemind;
            IsRead = viewModel.IsRead;
            Text = viewModel.Text;
        }

        public Notification()
        {

        }

        public int Id { get; set; }

        public int toDoListId { get; set; }

        public int recordId { get; set; }

        public string recordTitle { get; set; }

        public string recordDescription { get; set; }

        public string DueDate { get; set; }

        public string DateToRemind { get; set; }

        public bool IsRead { get; set; }  

        public string Text { get; set; }
    }
}
