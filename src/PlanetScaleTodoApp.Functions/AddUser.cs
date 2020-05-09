using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlanetScaleTodoApp.Functions.Models;
using PlanetScaleTodoApp.Functions.Dtos;

namespace PlanetScaleTodoApp.Functions
{
    public static class AddUser
    {
        [FunctionName("AddUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDto user = JsonConvert.DeserializeObject<UserDto>(requestBody);

            if (user != null)
            {
                string responseMessage = $"Welcome {user.Username}!";
                return new OkObjectResult(responseMessage);
            }
            else
            {
                return new BadRequestObjectResult("User not found in body");
            }
        }
    }
}
