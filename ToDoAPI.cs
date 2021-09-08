using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EdSmart
{
    public static class ToDoApi
    {
        private static List<Todo> items = new();

        [Function("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]
            HttpRequest req,
            ILogger log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));
            log.LogInformation("Creating a new todo list item");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription, IsCompleted = false };
            items.Add(todo);
            return new OkObjectResult(todo);
        }
    }
}