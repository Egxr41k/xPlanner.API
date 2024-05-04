using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record UserTaskRequest(
    string name, 
    bool isCompleted, 
    string createdAt, 
    string priority);

public class UserTaskService
{
    private readonly IRepository<UserTask> repository;
    private readonly IJwtProvider jwtProvider;

    public UserTaskService(
        IRepository<UserTask> repository,
        IJwtProvider jwtProvider)
    {
        this.repository = repository;
        this.jwtProvider = jwtProvider;
    }

    public async Task<List<UserTask>> GetTasks(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var tasks = await repository.GetAll();

        //TODO: fix data type at UserTask.UserId. string => int
        return tasks
            .Where(task => task.UserId == userId.ToString())
            .ToList();
    }

    public async Task<UserTask> CreateTask(
        UserTaskRequest userTask,
        HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var task = new UserTask()
        {
            //TODO: fix data type at UserTask.UserId. string => int
            UserId = userId.ToString(),
            Name = userTask.name,
            IsCompleted = userTask.isCompleted,
            CreatedAt = DateTime.UtcNow, //userTask.createdAt,
            Priority = userTask.priority
        };

        return await repository.Add(task);
    }

    public async Task<UserTask> UpdateTask(
        int id, 
        UserTaskRequest userTask, 
        HttpContext context)
    {
        var task = await repository.GetById(id);

        task.Name = userTask.name;
        task.IsCompleted = userTask.isCompleted;
        //task.CreatedAt = userTask.createdAt;
        task.Priority = userTask.priority;

        return await repository.Update(task);
    }

    public async Task<UserTask> DeleteTask(
        int id, 
        HttpContext context)
    {
        return await repository.Delete(id);
    }
}
