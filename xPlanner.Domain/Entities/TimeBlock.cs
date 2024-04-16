namespace xPlanner.Domain.Entities;

public class TimeBlock
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string Color { get; set; }
    public int Duration { get; set; }
    public int Order { get; set; }
}