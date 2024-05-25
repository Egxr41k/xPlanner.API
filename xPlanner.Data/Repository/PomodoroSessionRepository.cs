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

        session = dbContext.PomodoroSessions
            .FirstOrDefault(session => session.CreatedAt.Day == DateTime.Today.Day)!;

        var rounds = await CreateRounds(session);
        await dbContext.PomodoroRounds.AddRangeAsync(rounds);

        rounds = await dbContext.PomodoroRounds
            .Where(round => round.SessionId == session.Id)
            .OrderBy(round => round.Id)
            .ToListAsync();

        session.Rounds = rounds;

        await dbContext.SaveChangesAsync();

        return session;
    }

    public async Task<List<PomodoroRound>> CreateRounds(PomodoroSession session)
    {
        var settings = await dbContext.UsersSettings
            .FirstOrDefaultAsync(settings => settings.UserId == session.UserId);

        var roundsCount = settings?.PomodoroIntervalsCount ?? 0;
        var rounds = new List<PomodoroRound>();

        for (int i = 0; i < roundsCount; i++)
        {
            rounds.Add(new PomodoroRound
            {
                SessionId = session.Id,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            });
        }

        return rounds;
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
