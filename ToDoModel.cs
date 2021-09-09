using System;


public class Todo
{
    public string Id { get; set; } = new Guid().ToString("n");
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public string TaskDescription { get; set; }
    public bool IsCompleted { get; set; }
}

public class CreateToDoModel
{
    public string TaskDescription { get; set; }
}

public class TodoUpdateModel
{
    public string TaskDescription { get; set; }
    public bool IsCompleted { get; set; }
}