using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record UserDto (
    string name, 
    string email, 
    string pasword,
    int workInterval,
    int breakInterval,
    int intervalsCount);

public record Data(string label, string value);
public record MyProfileResponse(UserDto user, Data[] statistics);
public record UpdatedUserResponse(string email, string name);

public interface IUserService
{
    Task<bool> CheckIfUserExists(string email);
    Task<User> CreateUser(string email, string password);
    Task<User> DeleteUser(int userId);
    Task<User?> GetByEmail(string email);
    Task<User> GetById(int userId);
    Task<Data[]> GetStatistics(User user);
    Task<MyProfileResponse> GetUser(int userId);
    Task<User> GetUserByEmailAndPassword(string email, string password);
    Task<UpdatedUserResponse> UpdateUser(UserDto user, int userId);
}

public class UserService : IUserService
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

    public async Task<MyProfileResponse> GetUser(
        int userId)
    {
        var user = await repository.GetById(userId);

        var statistics = await GetStatistics(user);

        return new MyProfileResponse(
            new UserDto(
                user.Name, 
                user.Email, 
                user.Password, 
                user.Settings.PomodoroWorkInterval, 
                user.Settings.PomodoroBreakInterval,
                user.Settings.PomodoroIntervalsCount), 
            statistics);
    }

    public async Task<Data[]> GetStatistics(User user)
    {
        var totalTasks = user.Tasks.Count;

        var completedTasks = user.Tasks
            .Where(task => task.IsCompleted)
            .ToList()
            .Count;

        var todayTasks = user.Tasks
            .Where(task => task.CreatedAt.Day == DateTime.Today.Day)
            .ToList()
            .Count;

        var weekTasks = user.Tasks
            .Where(task => task.CreatedAt.AddDays(7) > DateTime.Today)
            .ToList()
            .Count;

        return [
            new Data("Total", totalTasks.ToString()),
            new Data("Completed tasks", completedTasks.ToString()),
            new Data("Today tasks", todayTasks.ToString()),
            new Data("Week tasks", weekTasks.ToString()),
        ];
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

            Settings = new UserSettings() // setting default settings
            {
                PomodoroBreakInterval = 10,
                PomodoroIntervalsCount = 7,
                PomodoroWorkInterval = 50,
            }
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

    public async Task<UpdatedUserResponse> UpdateUser(
        UserDto user, 
        int userId)
    {
        var existingUser = await repository.GetById(userId);

        existingUser.Name = user.name;
        existingUser.Email = user.email;
        existingUser.Settings.PomodoroWorkInterval = user.workInterval;
        existingUser.Settings.PomodoroBreakInterval = user.breakInterval;
        existingUser.Settings.PomodoroIntervalsCount = user.intervalsCount;
        
        existingUser.LastUpdatedAt = DateTime.UtcNow;

        await repository.Update(existingUser);

        return new UpdatedUserResponse(existingUser.Email, existingUser.Name);
    }

    public async Task<User> DeleteUser(
        int userId)
    {
        return await repository.Delete(userId);
    }
}
