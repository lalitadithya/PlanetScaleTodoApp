using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetScaleTodoApp.Functions.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public List<TodoItem> Items { get; set; }
    }
}
