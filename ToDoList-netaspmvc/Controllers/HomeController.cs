using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList_netaspmvc.Infrastructure;
using ToDoList_netaspmvc.Models;
using ToDoList_netaspmvc.Models.Repo;
using ToDoList_netaspmvc.Models.ViewModels;

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
            //List<ToDoListViewModel> listsViewModel = new List<ToDoListViewModel>();
            List<ToDoList> lists = await _toDoListRepository.toDoLists.OrderBy(x => x.Id).ToListAsync();
            /*for (int i = 0; i < lists.Count; i++)
            {
                listsViewModel.Add(
                    new ToDoListViewModel(lists[i].Id, lists[i].Name, lists[i].Description,
                    _toDoListRepository.records.Where(x => x.toDoList == lists[i].Name).OrderBy(x => x.Number).ToList()));
            }
            */
                
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
            if (ModelState.IsValid)
            {
                bool result = await _toDoListRepository.AddToDoList(list);

                if (result)
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

        //Get /home/edit/{id}
        public async Task<ActionResult> Edit(int id)
        {
            ToDoList list = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if(list == null)
            {
                return NotFound();
            }

            //List<Record> records = await _toDoListRepository.records.Where(x => x.Id == id).OrderBy(x => x.Number).ToListAsync();

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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
