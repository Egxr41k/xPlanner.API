using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record PomodoroRoundRequest(int totalSeconds, bool isCompleted);
public record PomodoroSessionRequest(bool isCompleted);
public class PomodoroService
{
    private readonly IRepository<PomodoroSession> repository;
    private readonly IRepository<User> userRepository;
    private readonly IJwtProvider jwtProvider;

    public PomodoroService(
        IRepository<PomodoroSession> repository,
        IJwtProvider jwtProvider,
        IRepository<User> userRepository)
    {
        this.repository = repository;
        this.userRepository = userRepository;
        this.jwtProvider = jwtProvider;
    }

    public async Task<PomodoroSession> GetTodaySession(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var sessions = await repository.GetAll();

        var todaySession = sessions.FirstOrDefault(session =>
            session.CreatedAt.Day == DateTime.Today.Day &&
            session.UserId == userId);

        todaySession?.Rounds.OrderBy(round => round.Id);

        return todaySession ?? throw new Exception();
    }

    public async Task<PomodoroSession> CreateSession(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);
        var user = await userRepository.GetById(userId);

        var session = new PomodoroSession()
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Rounds = CreateOrderedRounds(user.Settings.PomodoroIntervalsCount),
            IsCompleted = false,
        };

        return await repository.Add(session);
    }

    private List<PomodoroRound> CreateOrderedRounds(int length)
    {
        return new PomodoroRound[length].OrderBy(round => round.Id).ToList();
    }

    public async Task<PomodoroSession> UpdateSession(
        HttpContext context, 
        PomodoroSession session)
    {
        return await repository.Update(session);
    }

    public async Task<PomodoroSession> UpdateRound(
        int roundId,
        HttpContext context,
        PomodoroRoundRequest round)
    {
        var session = await GetTodaySession(context);

        var existingRound = session.Rounds
            .FirstOrDefault(round => round.Id == roundId);

        session.Rounds.Remove(existingRound);
        // TODO: fix TotalMinutes => TotalSeconds
        existingRound.TotalMinutes = round.totalSeconds;
        existingRound.IsCompleted = round.isCompleted;

        session.Rounds.Add(existingRound);

        return await repository.Update(session);
    }

    public async Task<PomodoroSession> DeleteSession(HttpContext context, int sessionId)
    {
        return await repository.Delete(sessionId);
    }
}
