namespace xPlanner.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public UserSettings Settings { get; set; }
    public ICollection<UserTask> Tasks { get; set; }
    public ICollection<TimeBlock> TimeBlocks { get; set; }
    public ICollection<PomodoroSession> Sessions { get; set; }
}

