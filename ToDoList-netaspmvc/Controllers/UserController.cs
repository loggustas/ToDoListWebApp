using Microsoft.AspNetCore.Mvc;
using ToDoList_DomainModel.ViewModels;

namespace ToDoList_netaspmvc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            return Ok();
        }
    }
}
