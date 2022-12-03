using ToDoList_netaspmvc.Models.ViewModels;

namespace ToDoList_netaspmvc.Models
{
    public class Record
    {
        public Record(RecordViewModel recordViewModel)
        {
            Id = recordViewModel.Id;
            toDoList = recordViewModel.toDoList;
            Number = recordViewModel.Number;
            Title = recordViewModel.Title;
            Description = recordViewModel.Description;
            DueDate = recordViewModel.DueDate;
            Status = recordViewModel.Status;
            toDoListID = recordViewModel.toDoListID;
        }

        public Record()
        {

        }

        public int Id { get; set; }

        public string toDoList { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string DueDate { get; set; }

        public string Status { get; set; }

        public int toDoListID { get; set; }
    }
}
