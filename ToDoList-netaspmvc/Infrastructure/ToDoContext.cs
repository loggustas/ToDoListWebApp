using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList_DomainModel.Models;

namespace ToDoList_netaspmvc.Infrastructure
{
    public class ToDoContext : IdentityDbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
