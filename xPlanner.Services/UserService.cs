using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record UserRequest (
    string name, 
    string email, 
    string pasword,
    int workInterval,
    int breakInterval,
    int intervalsCount);

public class UserService
{
    private readonly IRepository<User> repository;
    private readonly IJwtProvider jwtProvider;
    private readonly IPasswordHasher passwordHasher;


    public UserService(
        IRepository<User> userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher) 
    {
        repository = userRepository;
        this.jwtProvider = jwtProvider;
        this.passwordHasher = passwordHasher;
    }

    public async Task<User> GetUser(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var user = await repository.GetById(userId);

        return user ?? throw new UnauthorizedAccessException();
    }

    public async Task<User> UpdateUser(HttpContext context, UserRequest user)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        return await repository.Update(new User()
        {
            Id = userId,
            Name = user.name,
            Email = user.email,
            Password = passwordHasher.Generate(user.pasword),
            Settings = new UserSettings()
            {
                PomodoroWorkInterval = user.workInterval,
                PomodoroBreakInterval = user.breakInterval,
                PomodoroIntervalsCount = user.intervalsCount,
            },
            LastUpdatedAt = DateTime.UtcNow,
        });
    }
}
