using Microsoft.EntityFrameworkCore;
using ToDoList_DomainModel.Models;

namespace ToDoList_netaspmvc.Infrastructure
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
