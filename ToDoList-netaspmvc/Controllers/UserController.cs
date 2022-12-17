using Microsoft.AspNetCore.Mvc;
using ToDoList_DomainModel.ViewModels;

namespace ToDoList_netaspmvc.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            return Ok();
        }
    }
}
