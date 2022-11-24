using System.Collections.Generic;
using System.Linq;

namespace ToDoList_netaspmvc.Models
{
    public class RecordRepository : IRecordRepository
    {
        private List<Record> _recordsList;

        public RecordRepository()
        {
            //temp data
            _recordsList = new List<Record>()
            {
                new Record() { Id = 1, toDoList = "Gusto List", Description = "kazka padaryt", Title = "kazkas1", DueDate = "ryt", Number = 1, Status = "Nepadarytas"},
                new Record() { Id = 2, toDoList = "Gusto List", Description = "kazka padaryt", Title = "kazkas2", DueDate = "ryt", Number = 2, Status = "Nepadarytas"},
                new Record() { Id = 3, toDoList = "Gusto List", Description = "kazka padaryt", Title = "kazkas3", DueDate = "ryt", Number = 3, Status = "Nepadarytas"},

            };
        }

        public Record GetRecord(string toDoListName, int recordNumber)
        {
            return _recordsList.Where(x => x.toDoList.Equals(toDoListName) && x.Number == recordNumber).ToArray()[0];
        }

        public Record[] GetToDoList(string toDoListName)
        {
            return _recordsList.Where(x => x.toDoList.Equals(toDoListName)).ToArray();
        }
    }
}
