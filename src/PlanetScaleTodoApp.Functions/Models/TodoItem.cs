using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetScaleTodoApp.Functions.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Item { get; set; }
        public bool IsCompleted { get; set; }
    }
}
