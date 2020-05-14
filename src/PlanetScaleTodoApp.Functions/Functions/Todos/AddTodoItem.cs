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
using PlanetScaleTodoApp.Functions.Models;
using System.Security.Claims;

namespace PlanetScaleTodoApp.Functions.Functions.Todos
{
    public class AddTodoItem
    {
        private readonly CosmosClient cosmosClient;
        public AddTodoItem(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("AddTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
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
                if(user != null)
                {
                    var todoItemsContainer = cosmosClient.GetContainer("TodoApp", "TodoItems");
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    TodoItemDto todoItemDto = JsonConvert.DeserializeObject<TodoItemDto>(requestBody);
                    TodoItem todoItem = new TodoItem
                    {
                        Id = Guid.NewGuid(),
                        IsCompleted = todoItemDto.IsCompleted,
                        Item = todoItemDto.Item,
                        UserId = user.Id
                    };
                    await todoItemsContainer.CreateItemAsync(todoItem, new PartitionKey(todoItem.UserId.ToString()));
                    return new OkObjectResult($"{todoItem.Id}");
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
