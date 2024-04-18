using Microsoft.EntityFrameworkCore;
using xPlanner.Domain.Entities;

namespace xPlanner.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UsersSettings { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<TimeBlock> TimeBlocks { get; set; }
    public DbSet<PomodoroSession> PomodoroSessions { get; set; }
    public DbSet<PomodoroRound> PomodoroRounds { get; set; }
}
