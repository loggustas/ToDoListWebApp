using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoListDomainModel
{
    public class Record
    {
        public int Id { get; set; }

        public string toDoList { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string DueDate { get; set; }

        public string Status { get; set; }
    }
}
