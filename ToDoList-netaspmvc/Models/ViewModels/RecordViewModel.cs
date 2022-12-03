﻿namespace ToDoList_netaspmvc.Models.ViewModels
{
    public class RecordViewModel
    {
        public RecordViewModel(int id, string toDoList, int number, string title, string description, string dueDate, string status, int toDoListID, bool hideCompletedAfter)
        {
            Id = id;
            this.toDoList = toDoList;
            Number = number;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            this.toDoListID = toDoListID;
            this.hideCompletedAfter = hideCompletedAfter;
        }

        public RecordViewModel(Record record, bool hideCompletedAfter)
        {
            Id = record.Id;
            this.toDoList = record.toDoList;
            Number = record.Number;
            Title = record.Title;
            Description = record.Description;
            DueDate = record.DueDate;
            Status = record.Status;
            this.toDoListID = record.toDoListID;
            this.hideCompletedAfter = hideCompletedAfter;
        }

        public RecordViewModel()
        {

        }

        public int Id { get; set; }

        public string toDoList { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string DueDate { get; set; }

        public string Status { get; set; }

        public int toDoListID { get; set; }

        public bool hideCompletedAfter { get; set; }
    }
}