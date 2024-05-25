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
    Task<UserTask> CreateTask(UserTaskRequest userTask, int userId);
    Task<UserTask> DeleteTask(int id, int userId);
    Task<List<UserTask>> GetTasks(int userId);
    Task<UserTask> UpdateTask(int id, UserTaskRequest userTask, int userId);
}

public class UserTaskService : IUserTaskService
{
    private readonly IRepository<UserTask> repository;

    public UserTaskService(IRepository<UserTask> repository)
    {
        this.repository = repository;
    }

    public async Task<List<UserTask>> GetTasks(int userId)
    {
        var tasks = await repository.GetAll();

        return tasks
            .Where(task => task.UserId == userId)
            .ToList();
    }

    public async Task<UserTask> CreateTask(
        UserTaskRequest userTask,
        int userId)
    {
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
        int userId)
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
        int userId)
    {
        return await repository.Delete(id);
    }
}
