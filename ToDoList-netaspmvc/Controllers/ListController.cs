using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ToDoList_netaspmvc.Models;
using ToDoList_netaspmvc.Models.Repo;
using ToDoList_netaspmvc.Models.ViewModels;

namespace ToDoList_netaspmvc.Controllers
{
	public class ListController : Controller
	{
		private readonly IToDoListRepository _repository;

		public ListController(IToDoListRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Index(int id, bool hideCompleted, bool showDueToday)
		{
            ViewData["ListIDView"] = id;
            TempData["hideCompleted"] = hideCompleted;
            TempData["showDueToday"] = showDueToday;

            List<Record> records = _repository.GetAllRecords(id);

            ViewData["recordCount"] = records.Count;

            bool isListEmpty = records.Count == 0;
            ViewData["isListEmpty"] = isListEmpty;

            if (hideCompleted)
            {
                records = records.Where(x => !x.Status.Equals("Completed")).ToList();
            }
            if (showDueToday)
            {
                for (int i = records.Count - 1 ; i >= 0; i--)
                {
                    var recordDate = DateTime.ParseExact(records[i].DueDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var todayDate = DateTime.Today;

                    if (recordDate != todayDate)
                    {
                        records.RemoveAt(i);
                    }
                }
            }

            string? name = _repository.GetList(id)?.Name;
            if(name == null)
            {
                ViewData["ListName"] = "There is no such list with this id.";
                ViewData["ListExists"] = false;
            }
            else
            {
                ViewData["ListName"] = name;
                ViewData["ListExists"] = true;
            }

            return View(records);
		}

        public IActionResult Create(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            ViewData["ListIDCreate"] = id;
            ViewData["ListName"] = _repository.toDoLists.FirstOrDefault(x => x.Id == id).Name;
            ViewData["hideCompletedAfter"] = hideCompletedAfter;
            ViewData["showDueTodayAfter"] = showDueTodayAfter;

            return View();
        }

        //Post Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RecordViewModel recordViewModel)
        {
            Record record = new Record(recordViewModel);
            if (ModelState.IsValid)
            {
                bool result = await _repository.AddRecord(record);

                if (result)
                {
                    TempData["Success"] = "The record has been added!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while adding the record.";
                }

                return RedirectToAction("Index", "List", new { 
                    id = record.toDoListID,
                    hideCompleted = recordViewModel.hideCompletedAfter,
                    showDueToday = recordViewModel.showDueTodayAfter
                });
            }
            Console.WriteLine("Post:");
            Console.WriteLine("id: " + recordViewModel.toDoListID + "   ");
            return View(recordViewModel);
        }

        public async Task<ActionResult> ViewFull(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            Record record = await _repository.records.FirstOrDefaultAsync(x => x.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            ViewData["ListName"] = record.toDoList;
            ViewData["hideCompletedAfter"] = hideCompletedAfter;
            ViewData["showDueTodayAfter"] = showDueTodayAfter;

            return View(record);
        }

        public async Task<ActionResult> EditRecord(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            Record record = await _repository.records.FirstOrDefaultAsync(x => x.Id == id);

            RecordViewModel recordViewModel = new RecordViewModel(record, hideCompletedAfter, showDueTodayAfter);

            if (record == null)
            {
                return NotFound();
            }

            ViewData["ListName"] = record.toDoList;

            return View(recordViewModel);
        }

        //Post home/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRecord(RecordViewModel recordViewModel)
        {
            Record record = new Record(recordViewModel);

            if (ModelState.IsValid)
            {
                bool result = await _repository.UpdateRecord(record);

                if (result)
                {
                    TempData["Success"] = "The record has been updated!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while updating the record list.";
                }

                return RedirectToAction("Index", "List", new { 
                    id = record.toDoListID,
                    hideCompleted = recordViewModel.hideCompletedAfter,
                    showDueToday = recordViewModel.showDueTodayAfter
                });
            }

            return View(recordViewModel);
        }

        public async Task<ActionResult> DeleteRecord(int id, bool hideCompletedAfter, bool showDueTodayAfter)
		{
			int toDoListId = _repository.GetRecord(id).toDoListID;
            if (ModelState.IsValid)
            {
                bool result = await _repository.DeleteRecord(id);

                if (result)
                {
                    TempData["Success"] = "The record was successfully deleted!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while Editting list.";
                }
            }

            return RedirectToAction("Index", "List", new { 
                id = toDoListId,
                hideCompleted = hideCompletedAfter,
                showDueToday = showDueTodayAfter,
            });
        }
    }
}
