using xPlanner.Domain.Entities;
using xPlanner.Services;

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

    private static async Task<IResult> DeleteTask(
        int id,
        HttpContext context,
        UserTaskService service)
    {
        var result = await service.DeleteTask(id, context);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateTask(
        int id,
        UserTaskRequest userTask,
        HttpContext context,
        UserTaskService service)
    {
        var result = await service.UpdateTask(id, userTask, context);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateTask(
        UserTaskRequest userTask,
        HttpContext context,
        UserTaskService service)
    {
        var result = await service.CreateTask(userTask, context);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetTasks(
        HttpContext context,
        UserTaskService service)
    {
        var result = await service.GetTasks(context);
        return Results.Ok(result);
    }
}
