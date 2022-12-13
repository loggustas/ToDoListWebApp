using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDoList_DomainModel.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
