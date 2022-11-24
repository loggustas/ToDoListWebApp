namespace ToDoList_netaspmvc.Models
{
    public interface IRecordRepository
    {
        public Record[] GetToDoList(string toDoListName);
        public Record GetRecord(string toDoListName, int recordNumber);
    }
}
