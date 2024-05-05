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
    private readonly IJwtProvider jwtProvider;

    public TimeBlockService(
        IRepository<TimeBlock> repository,
        IJwtProvider jwtProvider)
    {
        this.repository = repository;
        this.jwtProvider = jwtProvider;
    }

    public async Task<List<TimeBlock>> GetTimeBlocks(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var timeBlock = await repository.GetAll();

        //TODO: fix data type at TimeBlock.UserId. string => int
        return timeBlock
            .Where(timeBlock => timeBlock.UserId == userId.ToString())
            .ToList();
    } 

    public async Task<TimeBlock> CreateTimeBlock(HttpContext context,
        TimeBlockRequest timeBlock)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];
        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        return await repository.Add(new TimeBlock()
        {
            UserId = userId.ToString(),
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

        if (existingTimeBlock == null)
        {
            throw new Exception();
        }

        existingTimeBlock.Name = timeBlock.name;
        existingTimeBlock.Color = timeBlock.color;
        existingTimeBlock.Duration = timeBlock.duration;
        existingTimeBlock.Order = timeBlock.order;

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
