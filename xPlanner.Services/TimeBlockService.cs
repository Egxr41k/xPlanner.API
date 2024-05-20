using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record TimeBlockRequest(
    string name, 
    string color, 
    int duration, 
    int order);

public record UpdateOrderRequest(string[] ids);

public class TimeBlockService
{
    private readonly IRepository<TimeBlock> repository;

    public TimeBlockService(IRepository<TimeBlock> repository)
    {
        this.repository = repository;
    }

    public async Task<List<TimeBlock>> GetTimeBlocks(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var timeBlock = await repository.GetAll();

        return timeBlock
            .Where(timeBlock => timeBlock.UserId == userId)
            .ToList();
    } 

    public async Task<TimeBlock> CreateTimeBlock(HttpContext context,
        TimeBlockRequest timeBlock)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        return await repository.Add(new TimeBlock()
        {
            UserId = userId,
            Name = timeBlock.name,
            Duration = timeBlock.duration,
            Order = timeBlock.order,
        });
    }

    public async Task<TimeBlock> UpdateTimeBlock(
        int id,
        HttpContext context,
        TimeBlockRequest timeBlock)
    {
        var existingTimeBlock = await repository.GetById(id);

        existingTimeBlock.Name = timeBlock.name;
        existingTimeBlock.Color = timeBlock.color;
        existingTimeBlock.Duration = timeBlock.duration;
        existingTimeBlock.Order = timeBlock.order;
        existingTimeBlock.LastUpdatedAt = DateTime.UtcNow;

        return await repository.Update(existingTimeBlock);
    }

    public async Task<List<TimeBlock>> UpdateTimeBlocksOrder(
        HttpContext context, 
        UpdateOrderRequest updateOrder)
    {
        var timeBlocks = await GetTimeBlocks(context); 

        for (int i = 0; i < timeBlocks.Count; i++)
        {
            //TODO: fix data type to set order without converting
            timeBlocks[i].Order = Convert.ToInt32(updateOrder.ids[i]);
            await repository.Update(timeBlocks[i]);
        }

        return timeBlocks;
    }

    public async Task<TimeBlock> DeleteTimeBlock(int id)
    {
        return await repository.Delete(id);
    }
}
