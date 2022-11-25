using System.Collections.Generic;

namespace ToDoList_netaspmvc.Models.ViewModels
{
    public class ToDoListViewModel
    {
        public ToDoListViewModel(int id, string name, string description, List<Record> records)
        {
            Id = id;
            Name = name;
            Description = description;
            Records = records;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Record> Records { get; set; }
    }
}
