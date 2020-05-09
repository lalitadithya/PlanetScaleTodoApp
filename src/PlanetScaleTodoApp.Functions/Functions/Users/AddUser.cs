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
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

namespace PlanetScaleTodoApp.Functions.Functions.Users
{
    public class AddUser
    {
        private readonly CosmosClient cosmosClient;
        public AddUser(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("AddUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDto userDto = JsonConvert.DeserializeObject<UserDto>(requestBody);

            if (userDto != null)
            {
                var container = cosmosClient.GetContainer("TodoApp", "Users");
                Models.User user = new Models.User
                {
                    Id = Guid.NewGuid(),
                    Username = userDto.Username
                };
                await container.CreateItemAsync(user, new PartitionKey(user.Id.ToString()));
                return new OkObjectResult($"{user.Id}");
            }
            else
            {
                return new BadRequestObjectResult("User not found in body");
            }
        }
    }
}
