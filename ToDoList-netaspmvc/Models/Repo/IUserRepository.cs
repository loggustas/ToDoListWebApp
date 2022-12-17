using ToDoList_DomainModel.ViewModels;

namespace ToDoList_netaspmvc.Models.Repo
{
    public interface IUserRepository
    {
        public bool RegisterUser(RegisterViewModel registerViewModel);

        public bool UsernameExists(string username);
    }
}
