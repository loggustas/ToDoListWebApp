using Microsoft.EntityFrameworkCore;
using ToDoList_DomainModel.Models;

namespace ToDoList_netaspmvc.Infrastructure.Context
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
