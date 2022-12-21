using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ToDoList_DomainModel.ViewModels;
using ToDoList_DomainModel.Models;
using ToDoList_netaspmvc.Models.Repo;
using ToDoList_DomainModel;

namespace ToDoList_netaspmvc.Controllers
{
	public class ListController : Controller
	{
		private readonly IToDoListRepository _repository;
        
        private readonly INotificationRepository _notificationRepository;

		public ListController(IToDoListRepository repository, INotificationRepository notificationRepository)
		{
			_repository = repository;
            _notificationRepository = notificationRepository;
		}

		public IActionResult Index(int id, bool hideCompleted, bool showDueToday)
		{
            if(CurrentUser.Id != null)
            {
                ViewData["ListIDView"] = id;
                TempData["hideCompleted"] = hideCompleted;
                TempData["showDueToday"] = showDueToday;

                List<Record> records = _repository.GetAllRecords(id);

                ViewData["recordCount"] = records.Count;

                bool isListEmpty = records.Count == 0;
                ViewData["isListEmpty"] = isListEmpty;

                List<Notification> notificationList = _notificationRepository.GetNotificationsForList(id);
                TempData["notificationList"] = notificationList;

                if (hideCompleted)
                {
                    records = records.Where(x => !x.Status.Equals("Completed")).ToList();
                }
                if (showDueToday)
                {
                    for (int i = records.Count - 1; i >= 0; i--)
                    {
                        var recordDate = DateTime.ParseExact(records[i].DueDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var todayDate = DateTime.Today;

                        if (recordDate != todayDate)
                        {
                            records.RemoveAt(i);
                        }
                    }
                }

                string name = _repository.GetList(id)?.Name;
                if (name == null)
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
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }

        public IActionResult Create(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            if(CurrentUser.Id != null)
            {
                ViewData["ListIDCreate"] = id;
                ViewData["ListName"] = _repository.toDoLists.FirstOrDefault(x => x.Id == id)?.Name;
                ViewData["hideCompletedAfter"] = hideCompletedAfter;
                ViewData["showDueTodayAfter"] = showDueTodayAfter;

                return View();
            }
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }

        //Post Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RecordViewModel recordViewModel)
        {
            if (CurrentUser.Id != null)
            {
                Record record = new Record(recordViewModel);
                if (ModelState.IsValid)
                {
                    bool result = await _repository.AddRecord(record);

                    if (result)
                    {
                        TempData["Success"] = "The task has been added!";
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong while adding the task.";
                    }

                    return RedirectToAction("Index", "List", new
                    {
                        id = record.toDoListID,
                        hideCompleted = recordViewModel.hideCompletedAfter,
                        showDueToday = recordViewModel.showDueTodayAfter
                    });
                }

                return View(recordViewModel);
            }
            return NotFound();
        }

        public IActionResult CreateNotification(int recordId, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            if (CurrentUser.Id != null)
            {
                Record record = _repository.GetRecord(recordId);
                NotificationViewModel notificationViewModel = new NotificationViewModel
                {
                    Id = 0,
                    toDoListId = record.toDoListID,
                    recordId = record.Id,
                    recordDescription = record.Description,
                    recordTitle = record.Title,
                    DueDate = record.DueDate,
                    IsRead = false
                };

                ViewData["hideCompletedAfter"] = hideCompletedAfter;
                ViewData["showDueTodayAfter"] = showDueTodayAfter;

                return View(notificationViewModel);
            }
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNotification(NotificationViewModel notificationViewModel)
        {
            if (CurrentUser.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Notification notification = new Notification(notificationViewModel);
                    bool result = _notificationRepository.Create(notification);

                    if (result)
                    {
                        TempData["Success"] = "The reminder has been added!";
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong while adding the reminder.";
                    }

                    return RedirectToAction("Index", "List", new
                    {
                        id = notificationViewModel.toDoListId,
                        hideCompleted = notificationViewModel.hideCompletedAfter,
                        showDueToday = notificationViewModel.showDueTodayAfter
                    });
                }

                return View(notificationViewModel);
            }
            return NotFound();
        }

        public async Task<ActionResult> ViewFull(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            if (CurrentUser.Id != null)
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
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }

        public async Task<ActionResult> EditRecord(int id, bool hideCompletedAfter, bool showDueTodayAfter)
        {
            if (CurrentUser.Id != null)
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
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }

        //Post home/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRecord(RecordViewModel recordViewModel)
        {
            if (CurrentUser.Id != null)
            {
                Record record = new Record(recordViewModel);

                if (ModelState.IsValid)
                {
                    bool result = await _repository.UpdateRecord(record);

                    if (result)
                    {
                        TempData["Success"] = "The task has been updated!";
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong while updating the task.";
                    }

                    return RedirectToAction("Index", "List", new
                    {
                        id = record.toDoListID,
                        hideCompleted = recordViewModel.hideCompletedAfter,
                        showDueToday = recordViewModel.showDueTodayAfter
                    });
                }

                return View(recordViewModel);
            }
            return NotFound();
        }

        public async Task<ActionResult> DeleteRecord(int id, bool hideCompletedAfter, bool showDueTodayAfter)
		{
            if (CurrentUser.Id != null)
            {
                int toDoListId = _repository.GetRecord(id).toDoListID;
                if (ModelState.IsValid)
                {
                    _notificationRepository.DeleteNotificationsForRecord(id);
                    bool result = await _repository.DeleteRecord(id);

                    if (result)
                    {
                        TempData["Success"] = "The task was successfully deleted!";
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong while deleting the task.";
                    }
                }

                return RedirectToAction("Index", "List", new
                {
                    id = toDoListId,
                    hideCompleted = hideCompletedAfter,
                    showDueToday = showDueTodayAfter,
                });
            }
            //return RedirectToAction("NoUser", "Home");
            return NotFound();
        }
    }
}
