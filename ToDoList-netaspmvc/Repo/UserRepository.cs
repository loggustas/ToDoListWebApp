using System.Diagnostics;
using System.Linq;
using ToDoList_DomainModel.Models;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc.Helpers;
using ToDoList_netaspmvc.Infrastructure.Context;

namespace ToDoList_netaspmvc.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoDbContext _context;

        public UserRepository(ToDoDbContext context)
        {
            _context = context;
        }
        public bool RegisterUser(RegisterViewModel registerViewModel)
        {
            User user = new User(registerViewModel.Username, PasswordHasher.Hash(registerViewModel.Password));

            _context.Users.Add(user);
            int result = _context.SaveChanges();

            if (result == 1)
            {
                return true;
            }

            return true;
        }

        public bool UsernameExists(string username)
        {
            User user = _context.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public int? VerifyUser(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            
            if (PasswordHasher.Verify(password, user.Password))
            {
                return user.Id;
            }

            return null;
        }
    }
}
