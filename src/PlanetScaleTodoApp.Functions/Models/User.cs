using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetScaleTodoApp.Functions.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        public string DisplayName { get; set; }
    }
}
