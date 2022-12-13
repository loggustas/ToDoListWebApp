using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_DomainModel.Models;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface IToDoListRepository
    {
        public IQueryable<ToDoList> toDoLists { get; }

        public IQueryable<Record> records { get; }

        public int? AddToDoList(ToDoList toDoList);

        public Task<bool> AddRecord(Record record);
        public Task<bool> UpdateList(ToDoList toDoList);

        public Task<bool> UpdateRecord(Record record);

        public Record GetRecord(int id);

        public ToDoList GetList(int id);

        public List<Record> GetAllRecords(int toDoListId);

        public Task<bool> DeleteList(int toDoListId);

        public Task<bool> DeleteRecord(int id);

        public void CopyList(int idCopyFrom, int idCopyTo);

        public void CopyRecord(Record record, int ListIdCopyTo);

        public int CountToDoListEntries(int toDoListId);
    }
}
