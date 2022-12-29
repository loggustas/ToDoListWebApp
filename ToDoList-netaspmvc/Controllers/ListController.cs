using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList_DomainModel;
using ToDoList_DomainModel.Models;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc.Repo;

namespace ToDoList_netaspmvc.Controllers
{
    public class ListController : Controller
    {
        private readonly IToDoListRepository _toDoListRepository;
        
        private readonly INotificationRepository _notificationRepository;


        public ListController(IToDoListRepository repo, INotificationRepository notificationRepository)
        {
            _toDoListRepository = repo;
            _notificationRepository = notificationRepository;
        }

        public async Task<ActionResult> Index()
        {
            if (CurrentUser.Id != null)
            {
                List<ToDoList> lists = await _toDoListRepository.GetUserLists(CurrentUser.Id.Value);

                List<ToDoListViewModel> modelListView = new List<ToDoListViewModel>();

                foreach (ToDoList list in lists)
                {
                    modelListView.Add(new ToDoListViewModel(list, _toDoListRepository.CountToDoListEntries(list.Id)));
                }

                if (modelListView.Count == 0)
                {
                    ViewData["NoLists"] = true;
                }
                else
                {
                    ViewData["NoLists"] = false;
                }

                return View(modelListView);
            }
            return NotFound();
        }

        public IActionResult Create()
        {
            if(CurrentUser.Id != null)
            {
                return View();
            }
            return NotFound();
        }

        //Post Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoList list)
        {
            if (CurrentUser.Id != null)
            {
                if (ModelState.IsValid)
                {
                    list.UserId = CurrentUser.Id.Value;
                    int? result = _toDoListRepository.AddToDoList(list);

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
            return NotFound();
        }

        public async Task<ActionResult> Copy(int id)
        {
            if (CurrentUser.Id != null)
            {
                ToDoList listToCopy = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
                if (listToCopy == null)
                {
                    return NotFound();
                }

                return View(listToCopy);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Copy(ToDoList list)
        {
            if (CurrentUser.Id != null)
            {
                if (ModelState.IsValid)
                {
                    ToDoList toDoListCopy = new ToDoList { Id = 0, UserId = list.UserId, Name = list.Name, Description = list.Description };

                    int? result = _toDoListRepository.AddToDoList(toDoListCopy);

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
            return NotFound();
        }

        //Get /home/edit/{id}
        public async Task<ActionResult> Edit(int id)
        {
            if (CurrentUser.Id != null)
            {
                ToDoList list = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
                if (list == null)
                {
                    return NotFound();
                }

                return View(list);
            }
            return View("NoUser");
        }

        //Post home/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ToDoList list)
        {
            if (CurrentUser.Id != null)
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
            return NotFound();
        }

        public async Task<ActionResult> DeleteList(int id)
        {
            if (CurrentUser.Id != null)
            {
                if (ModelState.IsValid)
                {
                    ToDoList list = await _toDoListRepository.toDoLists.FirstOrDefaultAsync(x => x.Id == id);
                    if (list == null)
                    {
                        return NotFound();
                    }

                    _notificationRepository.DeleteNotificationsForList(id);
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
            return View("NoUser");
        }

        public IActionResult NoUser()
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
