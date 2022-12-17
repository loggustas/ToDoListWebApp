using Microsoft.AspNetCore.Mvc;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc.Infrastructure;
using ToDoList_netaspmvc.Models.Repo;

namespace ToDoList_netaspmvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository repo)
        {
            userRepository = repo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                if (userRepository.UsernameExists(registerViewModel.Username))
                {
                    TempData["Error"] = "Username already exists.";
                }
                else
                {
                    bool result = userRepository.RegisterUser(registerViewModel);

                    if(result)
                    {
                        TempData["Success"] = "Registered the user.";
                    }
                    else
                    {
                        TempData["Error"] = "Some error occured while registering the user.";
                    }
                    return RedirectToAction("MainPage", "Home");
                }
            }

            return View();
        }
    }
}
