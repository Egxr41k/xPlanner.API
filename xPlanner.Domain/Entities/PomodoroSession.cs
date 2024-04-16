namespace xPlanner.Domain.Entities;

public class PomodoroSession
{
    public int Id { get; set; }
    public int UserId { set; get; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<PomodoroRound> Rounds { get; set; }
}
