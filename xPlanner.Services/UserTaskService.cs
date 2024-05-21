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

public interface IUserTaskService
{
    Task<UserTask> CreateTask(UserTaskRequest userTask, HttpContext context);
    Task<UserTask> DeleteTask(int id, HttpContext context);
    Task<List<UserTask>> GetTasks(HttpContext context);
    Task<UserTask> UpdateTask(int id, UserTaskRequest userTask, HttpContext context);
}

public class UserTaskService : IUserTaskService
{
    private readonly IRepository<UserTask> repository;

    public UserTaskService(IRepository<UserTask> repository)
    {
        this.repository = repository;
    }

    public async Task<List<UserTask>> GetTasks(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var tasks = await repository.GetAll();

        return tasks
            .Where(task => task.UserId == userId)
            .ToList();
    }

    public async Task<UserTask> CreateTask(
        UserTaskRequest userTask,
        HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var task = new UserTask()
        {
            UserId = userId,
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
        task.Priority = userTask.priority;
        task.LastUpdatedAt = DateTime.UtcNow;

        return await repository.Update(task);
    }

    public async Task<UserTask> DeleteTask(
        int id,
        HttpContext context)
    {
        return await repository.Delete(id);
    }
}
