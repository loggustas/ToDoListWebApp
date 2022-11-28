using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface IToDoListRepository
    {
        public IQueryable<ToDoList> toDoLists { get; }

        public IQueryable<Record> records { get; }

        public Task<bool> AddToDoList(ToDoList toDoList);

        public Task<bool> AddRecord(Record record);
        public Task<bool> Update(ToDoList toDoList);

        public Task<bool> Update(Record record);

        public List<Record> GetAllRecords(int id);
    }
}
