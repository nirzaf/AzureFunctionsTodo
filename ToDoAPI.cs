using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EdSmart
{
    public static class ToDoApi
    {
        private static List<Todo> items = new();

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]
            HttpRequest req,
            ILogger log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));
            log.LogInformation("Creating a new todo list item");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateToDoModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription, IsCompleted = false };
            items.Add(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting todo list items");
            return new OkObjectResult(items);
        }

        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")]
            HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Getting todo list item by Id : {id}");
            var todo = items.FirstOrDefault(c => c.Id == id);
            return todo == null ? new NotFoundResult() : new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")]
            HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Update to list item by Id : {0}", id);
            var todo = items.FirstOrDefault(c => c.Id == id);
            if (todo == null) return new NotFoundResult();
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            todo.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.TaskDescription)) todo.TaskDescription = updated.TaskDescription;
            return new OkObjectResult(todo);
        }


        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")]
            HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Update to list item by Id : {0}", id);
            var todo = items.FirstOrDefault(c => c.Id == id);
            if (todo == null) return new NotFoundResult();
            items.Remove(todo);
            return new OkResult();
        }
    }
}