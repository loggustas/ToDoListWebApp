namespace ToDoList_netaspmvc.Models.ViewModels
{
    public class ToDoListViewModel
    {
        public ToDoListViewModel(int id, string name, string description, int recordCount)
        {
            Id = id;
            Name = name;
            Description = description;
            RecordCount = recordCount;
        }

        public ToDoListViewModel(ToDoList toDoList, int recordCount)
        {
            Id = toDoList.Id;
            Name = toDoList.Name;
            Description = toDoList.Description;
            RecordCount = recordCount;
        }

        public ToDoListViewModel()
        {
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RecordCount { get; set; }
    }
}
