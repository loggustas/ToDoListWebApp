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
            if (string.IsNullOrWhiteSpace(record.Description))
            {
                record.Description = "No description.";
            }

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

        public async Task<int?> AddToDoList(ToDoList toDoList)
        {
            if (string.IsNullOrWhiteSpace(toDoList.Description))
            {
                toDoList.Description = "No description.";
            }

            toDoContext.ToDoList.Add(toDoList);
            await toDoContext.SaveChangesAsync();

            return toDoContext.ToDoList.FirstOrDefault(x => x.Name.Equals(toDoList.Name) && x.Description.Equals(toDoList.Description))?.Id;
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
            if (string.IsNullOrWhiteSpace(toDoList.Description))
            {
                toDoList.Description = "No description.";
            }

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

        public async Task<bool> UpdateRecord(Record record)
        {
            if (string.IsNullOrWhiteSpace(record.Description))
            {
                record.Description = "No description.";
            }

            toDoContext.Record.Update(record);
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

        public ToDoList GetList(int id)
        {
            return toDoContext.ToDoList.FirstOrDefault(x => x.Id == id);
        }

        public void CopyList(int idCopyFrom, int idCopyTo)
        {
            List<Record> recordsToCopy = this.GetAllRecords(idCopyFrom);
            
            for (int i = 0; i < recordsToCopy.Count; i++)
            {
                this.CopyRecord(recordsToCopy[i], idCopyTo);
            }
        }

        public void CopyRecord(Record record, int ListIdCopyTo)
        {
            string toDoListName = this.GetList(ListIdCopyTo).Name;

            Record recordCopy = new Record() {
                toDoListID = ListIdCopyTo,
                Description = record.Description,
                DueDate = record.DueDate,
                Status = record.Status,
                Title = record.Title,
                toDoList = toDoListName,
            };

            toDoContext.Record.Add(recordCopy);
            toDoContext.SaveChanges();
        }

        public int CountToDoListEntries(int toDoListId)
        {
            return toDoContext.Record.Count(x => x.toDoListID == toDoListId);
        }
    }
}
