using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ToDoList_DomainModel
{
    public static class CurrentUser
    {
        public static int? Id { get; set; }
        public static string Username { get; set; }

        public static bool isAdmin { get; set; }

        public static void Login(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public static void Logout()
        {
            Id = null;
            Username = null;
        }
    }
}
