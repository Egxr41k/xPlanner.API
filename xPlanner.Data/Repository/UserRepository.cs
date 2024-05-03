using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<User>> GetAll()
    {
        return await dbContext.Users
            .Include(user => user.Tasks)
            .Include(user => user.TimeBlocks)
            .Include(user => user.Sessions)
            .Include(user => user.Settings)
            .ToListAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await dbContext.Users
            .Include(user => user.Tasks)
            .Include(user => user.TimeBlocks)
            .Include(user => user.Sessions)
            .Include(user => user.Settings)
            .FirstOrDefaultAsync(u => u.Id == id) ?? 
            throw new ObjectNotFoundException();
    }

    public async Task<User> Add(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.Name = user.Email; // Set name to email by default

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> Update(User user)
    {
        var existingUser = await GetById(user.Id);

        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.LastUpdatedAt = DateTime.UtcNow;

        // Update related entities if necessary (e.g., Tasks, TimeBlocks, Sessions)

        await dbContext.SaveChangesAsync();

        return existingUser;
    }

    public async Task<User> Delete(int id)
    {
        var user = await GetById(id);

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();

        return user;
    }
}
