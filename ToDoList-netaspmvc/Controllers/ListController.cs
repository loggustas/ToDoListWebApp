using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList_netaspmvc.Models;
using ToDoList_netaspmvc.Models.Repo;

namespace ToDoList_netaspmvc.Controllers
{
	public class ListController : Controller
	{
		private readonly IToDoListRepository _repository;

		public ListController(IToDoListRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Index(int id)
		{
			List<Record> records = _repository.GetAllRecords(id);

			/*
			if(records == null || records.Count == 0)
			{
				throw new ArgumentException("There were no records found in a list.");
			}
			*/

			ViewData["ListName"] = _repository.GetList(id).Name;
			return View(records);
		}

        public async Task<ActionResult> DeleteRecord(int id)
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

            return RedirectToAction("Index", "List", new { id = toDoListId});
        }
    }
}
