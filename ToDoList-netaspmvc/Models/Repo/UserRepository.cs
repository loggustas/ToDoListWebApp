using System.Linq;
using ToDoList_DomainModel.Models;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc.Infrastructure;

namespace ToDoList_netaspmvc.Models.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoContext _context;

        public UserRepository(ToDoContext context)
        {
            _context = context;
        }
        public bool RegisterUser(RegisterViewModel registerViewModel)
        {
            User user = new User(registerViewModel);

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
    }
}
