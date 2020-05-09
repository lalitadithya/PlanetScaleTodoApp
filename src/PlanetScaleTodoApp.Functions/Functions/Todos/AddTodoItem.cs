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
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Guid userId = Guid.Parse(req.Query["userId"]);
            var usersContainer = cosmosClient.GetContainer("TodoApp", "Users");
            FeedIterator<Models.User> userFeedIterator = usersContainer.GetItemQueryIterator<Models.User>($"SELECT * FROM U where U.id = '{userId}'");
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
                        UserId = userId
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
