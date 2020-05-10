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
using System.Security.Claims;
using System.Linq;

namespace PlanetScaleTodoApp.Functions.Functions.Users
{
    public class GetUserId
    {
        private readonly CosmosClient cosmosClient;
        public GetUserId(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("GetUserId")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log, ClaimsPrincipal claimsPrincipal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string emailId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            string name = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "name").Value;

            var usersContainer = cosmosClient.GetContainer("TodoApp", "Users");
            FeedIterator<Models.User> userFeedIterator = usersContainer.GetItemQueryIterator<Models.User>($"SELECT * FROM U where U.username = '{emailId}'");
            if (userFeedIterator.HasMoreResults)
            {
                Models.User user = (await userFeedIterator.ReadNextAsync()).Resource.FirstOrDefault();
                if (user != null)
                {
                    return new OkObjectResult($"{user.Id}");
                }
                else
                {
                    Models.User newUser = new Models.User
                    {
                        Id = Guid.NewGuid(),
                        Username = emailId,
                        DisplayName = name
                    };
                    await usersContainer.CreateItemAsync(newUser, new PartitionKey(newUser.Username.ToString()));
                    return new OkObjectResult($"{newUser.Id}");
                }
            }
            else
            {
                Models.User user = new Models.User
                {
                    Id = Guid.NewGuid(),
                    Username = emailId,
                    DisplayName = name
                };
                await usersContainer.CreateItemAsync(user, new PartitionKey(user.Username.ToString()));
                return new OkObjectResult($"{user.Id}");
            }
        }
    }
}
