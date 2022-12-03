using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

		public IActionResult Index(int id, bool hideCompleted)
		{
            ViewData["ListIDView"] = id;
            TempData["hideCompleted"] = hideCompleted;

            List<Record> records = _repository.GetAllRecords(id);

            bool isListEmpty = records.Count() == 0;
            ViewData["isListEmpty"] = isListEmpty;

            if (hideCompleted)
            {
                records = records.Where(x => !x.Status.Equals("Completed")).ToList();
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

        public IActionResult Create(int id)
        {
            ViewData["ListIDCreate"] = id;
            ViewData["ListName"] = _repository.toDoLists.FirstOrDefault(x => x.Id == id).Name;

            return View();
        }

        //Post Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Record record)
        {
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

                return RedirectToAction("Index", "List", new { id = record.toDoListID });
            }

            return View(record);
        }

        public async Task<ActionResult> ViewFull(int id, bool hideCompletedAfter)
        {
            Record record = await _repository.records.FirstOrDefaultAsync(x => x.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            ViewData["ListName"] = record.toDoList;
            ViewData["hideCompletedAfter"] = hideCompletedAfter;

            return View(record);
        }

        public async Task<ActionResult> EditRecord(int id, bool hideCompletedAfter)
        {
            Record record = await _repository.records.FirstOrDefaultAsync(x => x.Id == id);

            RecordViewModel recordViewModel = new RecordViewModel(record, hideCompletedAfter);

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

                return RedirectToAction("Index", "List", new { id = record.toDoListID, hideCompleted = recordViewModel.hideCompletedAfter});
            }

            return View(record);
        }

        public async Task<ActionResult> DeleteRecord(int id, bool hideCompletedAfter)
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

            return RedirectToAction("Index", "List", new { id = toDoListId, hideCompleted = hideCompletedAfter});
        }
    }
}
