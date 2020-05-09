using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetScaleTodoApp.Functions.Dtos
{
    public class TodoItemDto
    {
        public string Id { get; set; }
        public string Item { get; set; }

        public bool IsCompleted { get; set; }
    }
}
