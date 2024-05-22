using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record PomodoroRoundRequest(int totalSeconds, bool isCompleted);
public record PomodoroSessionRequest(bool isCompleted);

public interface IPomodoroService
{
    Task<PomodoroSession> CreateSession(int userId);
    Task<PomodoroSession> DeleteSession(int sessionId, int userId);
    Task<PomodoroSession> GetTodaySession(int userId);
    Task<PomodoroRound> UpdateRound(int roundId, PomodoroRoundRequest round,int userId);
    Task<PomodoroSession> UpdateSession(int sessionId, PomodoroSessionRequest session, int userId);
}

public class PomodoroService : IPomodoroService
{
    private readonly IRepository<PomodoroSession> repository;

    public PomodoroService(IRepository<PomodoroSession> repository)
    {
        this.repository = repository;
    }

    public async Task<PomodoroSession> GetTodaySession(
        int userId)
    {
        var sessions = await repository.GetAll();

        var todaySession = sessions.FirstOrDefault(session =>
            session.CreatedAt.Day == DateTime.Today.Day &&
            session.UserId == userId);

        todaySession?.Rounds.OrderBy(round => round.Id);

        return todaySession ?? await CreateSession(userId);
    }

    public async Task<PomodoroSession> CreateSession(
        int userId)
    {
        var session = new PomodoroSession()
        {
            UserId = userId,
            //Rounds = CreateOrderedRounds(user?.Settings?.PomodoroIntervalsCount ?? 6),
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
        };

        return await repository.Add(session);
    }

    private List<PomodoroRound> CreateOrderedRounds(int length)
    {
        return new PomodoroRound[length].ToList();
    }

    public async Task<PomodoroSession> UpdateSession(
        int sessionId,
        PomodoroSessionRequest session,
        int userId)
    {
        var existingSession = await repository.GetById(sessionId);
        existingSession.IsCompleted = session.isCompleted;
        existingSession.LastUpdatedAt = DateTime.UtcNow;

        return await repository.Update(existingSession);
    }

    public async Task<PomodoroRound> UpdateRound(
        int roundId,
        PomodoroRoundRequest round,
        int userId)
    {
        var session = await GetTodaySession(userId);

        var existingRound = session.Rounds
            .FirstOrDefault(round => round.Id == roundId);

        session.Rounds.Remove(existingRound);
        // TODO: fix TotalMinutes => TotalSeconds
        existingRound.TotalMinutes = round.totalSeconds;
        existingRound.IsCompleted = round.isCompleted;

        session.Rounds.Add(existingRound);

        await repository.Update(session);

        return existingRound;
    }

    public async Task<PomodoroSession> DeleteSession(
        int sessionId,
        int userId)
    {
        return await repository.Delete(sessionId);
    }
}
