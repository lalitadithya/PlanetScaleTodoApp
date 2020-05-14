using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Linq;
using PlanetScaleTodoApp.Functions.Dtos;
using System.Collections.Generic;
using PlanetScaleTodoApp.Functions.Models;
using System.Security.Claims;

namespace PlanetScaleTodoApp.Functions.Functions.Todos
{
    public class ToggleTodoItem
    {
        private readonly CosmosClient cosmosClient;
        public ToggleTodoItem(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("ToggleTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
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
                    Guid todoItemId = Guid.Parse(req.Query["todoItemId"]);
                    var todoItemsContainer = cosmosClient.GetContainer("TodoApp", "TodoItems");
                    FeedIterator<TodoItem> todoItemFeedIterator = todoItemsContainer.GetItemQueryIterator<TodoItem>($"SELECT * FROM T where T.userId = '{user.Id}' AND T.id = '{todoItemId}'");
                    while (todoItemFeedIterator.HasMoreResults)
                    {
                        TodoItem todoItem = (await todoItemFeedIterator.ReadNextAsync()).Resource.FirstOrDefault();
                        if (todoItem != null)
                        {
                            todoItem.IsCompleted = !todoItem.IsCompleted;
                            await todoItemsContainer.ReplaceItemAsync(todoItem, todoItem.Id.ToString(), new PartitionKey(todoItem.UserId.ToString()));
                        }
                        else
                        {
                            return new BadRequestObjectResult("todo item not found");
                        }
                    }

                    return new OkResult();
                }
                else
                {
                    return new BadRequestObjectResult("user is null");
                }
            }
            else
            {
                return new NotFoundObjectResult("user not found");
            }
        }
    }
}
