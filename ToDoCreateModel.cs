using System;

public class ToDoCreateModel
{
    public string TaskDescription { get; set; }
}

public class Todo
{
    public string Id { get; set; } = new Guid().ToString("n");
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public string TaskDescription { get; set; }
    public bool IsCompleted { get; set; }
}