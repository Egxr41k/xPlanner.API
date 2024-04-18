using Microsoft.EntityFrameworkCore;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        if (user == null) return;
        if (dbContext.Users.Contains(user)) return;

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        User? user = await dbContext.Users.FindAsync(id);

        if (user == null) return;
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAll()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await dbContext.Users.FindAsync(id) ?? new User();
    }

    public async Task Update(User user)
    {
        if (user == null) return;
        if (!dbContext.Users.Contains(user)) return;

        dbContext.Entry(user).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }
}
