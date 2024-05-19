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
    private readonly IPasswordHasher passwordHasher;

    public UserService(
        IRepository<User> repository,
        IPasswordHasher passwordHasher)
    {
        this.repository = repository;
        this.passwordHasher = passwordHasher;
    }

    public async Task<User> GetUser(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var user = await repository.GetById(userId);

        return user ?? throw new UnauthorizedAccessException();
    }

    public async Task<bool> CheckIfUserExists(string email)
    {
        var existingUser = await GetByEmail(email);
        return existingUser != null;
    }

    public async Task<User> CreateUser(string email, string password)
    {
        var user = new User
        {
            Email = email,
            Password = passwordHasher.Generate(password),
            Name = email, // Set name to email by default
            CreatedAt = DateTime.UtcNow,
        };

        await repository.Add(user);

        return user;
    }

    public async Task<User> GetUserByEmailAndPassword(string email, string password)
    {
        var user = await GetByEmail(email);

        if (user == null || !passwordHasher.Verify(password, user.Password))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        return user;
    }

    public async Task<User> GetById(int userId)
    {
        return await repository.GetById(userId);
    }

    public async Task<User?> GetByEmail(string email)
    {
        var users = await repository.GetAll();

        return users.FirstOrDefault(u => u.Email == email);
        //?? throw new ObjectNotFoundException();
    }

    public async Task<User> UpdateUser(HttpContext context, UserRequest user)
    {
        var userIdClaim = context.User.Claims
            .FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var existingUser = await repository.GetById(userId);

        existingUser.Name = user.name;
        existingUser.Email = user.email;
        existingUser.Password = passwordHasher.Generate(user.pasword);
        existingUser.Settings = new UserSettings()
        {
            PomodoroWorkInterval = user.workInterval,
            PomodoroBreakInterval = user.breakInterval,
            PomodoroIntervalsCount = user.intervalsCount,
        };
        existingUser.LastUpdatedAt = DateTime.UtcNow;
        
        return await repository.Update(existingUser);
    }

    public async Task<User> DeleteSession(
        int userId,
        HttpContext context)
    {
        return await repository.Delete(userId);
    }
}
