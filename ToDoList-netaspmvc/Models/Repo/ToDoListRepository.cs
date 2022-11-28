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

        public List<Record> GetAllRecords(int toDoListId)
        {
            return toDoContext.Record.Where(x => x.toDoListID == toDoListId).ToList();
        }

        public async Task<bool> DeleteList(int toDoListId)
        {
            List<Record> recordsToRemove = this.GetAllRecords(toDoListId);
            for (int i = 0; i < recordsToRemove.Count; i++)
            {
                toDoContext.Record.Remove(recordsToRemove[i]);
            }

            ToDoList listToDelete = toDoContext.ToDoList.FirstOrDefault(x => x.Id == toDoListId);
            int deletedEntryCount = recordsToRemove.Count + 1;

            toDoContext.ToDoList.Remove(listToDelete);
            int actuallyDeletedEntryCount = await toDoContext.SaveChangesAsync();

            if (deletedEntryCount == actuallyDeletedEntryCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteRecord(int id)
        {
            toDoContext.Record.Remove(this.GetRecord(id));
            int recordsRemoved = await toDoContext.SaveChangesAsync();

            if (recordsRemoved == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Record GetRecord(int id)
        {
            return this.toDoContext.Record.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> UpdateList(ToDoList toDoList)
        {
            toDoContext.ToDoList.Update(toDoList);
            int numOfUpdates = await toDoContext.SaveChangesAsync();

            if (numOfUpdates == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<bool> UpdateRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public ToDoList GetList(int id)
        {
            return toDoContext.ToDoList.FirstOrDefault(x => x.Id == id);
        }
    }
}
