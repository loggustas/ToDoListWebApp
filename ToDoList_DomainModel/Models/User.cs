using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ToDoList_DomainModel.ViewModels;

namespace ToDoList_DomainModel.Models
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            IsAdmin = false;
        }
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
