using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_netaspmvc.Infrastructure;

namespace ToDoList_netaspmvc.Models.Repo
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoContext toDoContext;

        public ToDoListRepository(ToDoContext ctx)
        {
            this.toDoContext = ctx;
        }

        public IQueryable<ToDoList> toDoLists => toDoContext.ToDoList;

        public IQueryable<Record> records => toDoContext.Record;

        public async Task<bool> AddRecord(Record record)
        {
            toDoContext.Record.Add(record);
            int numOfRecs = await toDoContext.SaveChangesAsync();

            if(numOfRecs == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddToDoList(ToDoList toDoList)
        {
            toDoContext.ToDoList.Add(toDoList);
            int numOfLists = await toDoContext.SaveChangesAsync();

            if(numOfLists == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Update(ToDoList toDoList)
        {
            toDoContext.ToDoList.Update(toDoList);
            int numOfUpdates = await toDoContext.SaveChangesAsync();

            if(numOfUpdates == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
