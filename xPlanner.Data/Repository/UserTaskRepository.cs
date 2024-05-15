using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

public class UserTaskRepository : IRepository<UserTask>
{
    private readonly AppDbContext dbContext;

    public UserTaskRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UserTask> Add(UserTask task)
    {
        await dbContext.Tasks.AddAsync(task);
        await dbContext.SaveChangesAsync();

        return task;
    }

    public async Task<UserTask> Delete(int id)
    {
        var task = await GetById(id);

        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync();

        return task;
    }

    public async Task<List<UserTask>> GetAll()
    {
        return await dbContext.Tasks.ToListAsync();
    }

    public async Task<UserTask> GetById(int id)
    {
        return await dbContext.Tasks
            .FirstOrDefaultAsync(task => task.Id == id) ??
             throw new ObjectNotFoundException();
    }

    public async Task<UserTask> Update(UserTask task)
    {
        var existingTask = await GetById(task.Id);
       
        existingTask = task;

        await dbContext.SaveChangesAsync();

        return existingTask;
    }
}
