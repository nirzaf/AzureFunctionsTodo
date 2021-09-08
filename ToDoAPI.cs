using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Edsmart
{
    public static class ToDoAPI
    {
        private static List<Todo> items = new();

        [Function("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]
            HttpRequest req,
            TraceWriter log)
        {
            log.LogInformation("Creating a new todo list item");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription, IsCompleted = false };
            items.Add(todo);
            return new OkObjectResult(todo);
        }
    }
}