using Microsoft.AspNetCore.Mvc;
using ToDoList_DomainModel;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc.Repo;

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
                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                int? UserId = userRepository.VerifyUser(loginViewModel.Username, loginViewModel.Password);
                if (UserId == null)
                {
                    TempData["Error"] = "Incorrect account details!";
                }
                else
                {
                    TempData["Success"] = "Logged in.";
                    CurrentUser.Login(UserId.Value, loginViewModel.Username);
                    return RedirectToAction("Index", "List");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Loggout()
        {
            CurrentUser.Logout();
            return RedirectToAction("Login");
        }
    }
}
