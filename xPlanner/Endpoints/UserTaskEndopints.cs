namespace xPlanner.Endpoints;

public static class UserTaskEndopints
{
    public static void MapUserTaskEndopints(this WebApplication app)
    {
        app.MapGet("api/user/tasks", GetTasks);
        app.MapPost("api/user/tasks", CreateTask);
        app.MapPut("api/user/tasks{id:int}", UpdateTask);
        app.MapDelete("api/user/tasks/{id:int}", DeleteTask);
    }

    private static async Task DeleteTask(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateTask(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task CreateTask(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task GetTasks(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
