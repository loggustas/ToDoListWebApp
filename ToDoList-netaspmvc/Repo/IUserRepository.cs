using ToDoList_DomainModel.ViewModels;

namespace ToDoList_netaspmvc.Repo
{
    public interface IUserRepository
    {
        public bool RegisterUser(RegisterViewModel registerViewModel);

        public bool UsernameExists(string username);

        public int? VerifyUser(string username, string password);
    }
}
