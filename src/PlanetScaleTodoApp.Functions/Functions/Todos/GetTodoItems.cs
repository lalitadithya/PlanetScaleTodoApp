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
using PlanetScaleTodoApp.Functions.Models;
using System.Collections.Generic;
using PlanetScaleTodoApp.Functions.Dtos;

namespace PlanetScaleTodoApp.Functions.Functions.Todos
{
    public class GetTodoItems
    {
        private readonly CosmosClient cosmosClient;
        public GetTodoItems(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("GetTodoItems")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Guid userId = Guid.Parse(req.Query["userId"]);
            var usersContainer = cosmosClient.GetContainer("TodoApp", "Users");
            FeedIterator<Models.User> userFeedIterator = usersContainer.GetItemQueryIterator<Models.User>($"SELECT * FROM U where U.id = '{userId}'");
            if (userFeedIterator.HasMoreResults)
            {
                Models.User user = (await userFeedIterator.ReadNextAsync()).Resource.FirstOrDefault();
                if (user != null)
                {
                    var todoItemsContainer = cosmosClient.GetContainer("TodoApp", "TodoItems");

                    FeedIterator<TodoItem> todoItemFeedIterator = todoItemsContainer.GetItemQueryIterator<TodoItem>($"SELECT * FROM T where T.userId = '{user.Id}'");
                    List<TodoItemDto> todoItemDtos = new List<TodoItemDto>();
                    while (todoItemFeedIterator.HasMoreResults)
                    {
                        IEnumerable<TodoItem> todoItems = (await todoItemFeedIterator.ReadNextAsync()).Resource;
                        foreach(var todoItem in todoItems)
                        {
                            todoItemDtos.Add(new TodoItemDto
                            {
                                Item = todoItem.Item,
                                IsCompleted = todoItem.IsCompleted
                            });
                        }
                    }
                    
                    return new OkObjectResult($"{JsonConvert.SerializeObject(todoItemDtos)}");
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
