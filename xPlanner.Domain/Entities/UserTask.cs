namespace xPlanner.Domain.Entities;

public class UserTask
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public string Priority { get; set; }
}
