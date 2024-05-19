using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

public class PomodoroSessionRepository : IRepository<PomodoroSession>
{
    private readonly AppDbContext dbContext;

    public PomodoroSessionRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<PomodoroSession> Add(PomodoroSession session)
    {
        await dbContext.PomodoroSessions.AddAsync(session);
        await dbContext.SaveChangesAsync();

        return session;
    }

    public async Task<PomodoroSession> Delete(int id)
    {
        var session = await GetById(id);

        dbContext.PomodoroSessions.Remove(session);
        await dbContext.SaveChangesAsync();

        return session;
    }

    public async Task<List<PomodoroSession>> GetAll()
    {
        return await dbContext.PomodoroSessions
            .Include(s => s.Rounds).ToListAsync();
    }

    public async Task<PomodoroSession> GetById(int id)
    {
        return await dbContext.PomodoroSessions
             .Include(session => session.Rounds)
             .FirstOrDefaultAsync(session => session.Id == id) ??
             throw new ObjectNotFoundException();
    }

    public async Task<PomodoroSession> Update(PomodoroSession session)
    {
        var existingSession = await GetById(session.Id);

        existingSession = session;

        await dbContext.SaveChangesAsync();

        return existingSession;
    }
}
