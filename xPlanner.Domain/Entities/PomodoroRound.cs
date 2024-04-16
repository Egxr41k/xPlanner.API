namespace xPlanner.Domain.Entities;

public class PomodoroRound
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int TotalMinutes { get; set; }
}
