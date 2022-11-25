using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList_netaspmvc.Models;

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
    }
}
