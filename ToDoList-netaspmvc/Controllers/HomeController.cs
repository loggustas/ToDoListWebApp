﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList_netaspmvc.Models;
using ToDoList_netaspmvc.Models.Repo;

namespace ToDoList_netaspmvc.Controllers
{
    public class HomeController : Controller
    {
        private IToDoListRepository _toDoListRepository;

        public HomeController(IToDoListRepository repo)
        {
            _toDoListRepository = repo;
        }

        public async Task<ActionResult> Index()
        {
            List<ToDoList> lists = await _toDoListRepository.toDoLists.OrderBy(x => x.Id).ToListAsync();

            return View(lists);  
        }

        public IActionResult Create()
        {
            return View();
        }

        //Post Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoList list)
        {
            Console.WriteLine(list.Id + "   " + list.Name + "    " + list.Description);
            if (ModelState.IsValid)
            {
                int? result = await _toDoListRepository.AddToDoList(list);

                if (result != null)
                {
                    TempData["Success"] = "The list has been added!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while Editting list.";
                }

                return RedirectToAction("Index");
            }

            return View(list);
        }

        public async Task<ActionResult> Copy(int id)
        {
            ToDoList listToCopy = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if(listToCopy == null)
            {
                return NotFound();
            }

            ViewData["ListToCopyID"] = id;

            return View(listToCopy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Copy(ToDoList list)
        {
            if (ModelState.IsValid)
            {
                ToDoList toDoListCopy = new ToDoList { Id = 0, Name = list.Name, Description = list.Description };

                int? result = await _toDoListRepository.AddToDoList(toDoListCopy);

                if (result != null)
                {
                    TempData["Success"] = "The list has been copied!";
                    _toDoListRepository.CopyList(list.Id, result.Value);
                }
                else
                {
                    TempData["Error"] = "Something went wrong while Copying list.";
                }

                return RedirectToAction("Index");
            }

            return View(list);
        }

        //Get /home/edit/{id}
        public async Task<ActionResult> Edit(int id)
        {
            ToDoList list = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if(list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        //Post home/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ToDoList list)
        {
            if (ModelState.IsValid)
            {
                bool result = await _toDoListRepository.UpdateList(list);

                if (result)
                {
                    TempData["Success"] = "The list has been updated!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while Editting list.";
                }

                return RedirectToAction("Index");
            }

            return View(list);
        }

        public async Task<ActionResult> DeleteList(int id)
        {
            if (ModelState.IsValid)
            {
                bool result = await _toDoListRepository.DeleteList(id);

                if (result)
                {
                    TempData["Success"] = "The list has been deleted!";
                }
                else
                {
                    TempData["Error"] = "Something went wrong while deleting the list.";
                }
            }

            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
