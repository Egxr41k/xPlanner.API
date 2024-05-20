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
        await dbContext.PomodoroRounds.AddRangeAsync(session.Rounds);
        await dbContext.SaveChangesAsync();

        return session;
    }

    public async Task<PomodoroSession> Delete(int id)
    {
        var session = await GetById(id);

        dbContext.PomodoroSessions.Remove(session);
        dbContext.PomodoroRounds.RemoveRange(session.Rounds);
        await dbContext.SaveChangesAsync();

        return session;
    }

    public async Task<List<PomodoroSession>> GetAll()
    {
        return await dbContext.PomodoroSessions
            .Include(s => s.Rounds)
            .ToListAsync();
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

        await dbContext.PomodoroRounds
            .Where(round => round.SessionId == session.Id)
            .ForEachAsync(round => round = session.Rounds
                .FirstOrDefault(newRound => round.Id == newRound.Id)!);

        existingSession = session;

        existingSession.Rounds = await dbContext.PomodoroRounds
            .Where(round => round.SessionId == session.Id)
            .ToListAsync();

        await dbContext.SaveChangesAsync();

        return existingSession;
    }
}
