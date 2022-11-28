using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
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

			if(records == null || records.Count == 0)
			{
				throw new ArgumentException("There were no records found in a list.");
			}

			ViewData["ListName"] = records[0].toDoList;
			return View(records);
		}
	}
}
