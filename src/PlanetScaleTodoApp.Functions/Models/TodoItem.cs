using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetScaleTodoApp.Functions.Models
{
    public class TodoItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "item")]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "isCompleted")]
        public bool IsCompleted { get; set; }
    }
}
