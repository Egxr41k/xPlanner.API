using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class TimeBlockEndpoints
{
    public static void MapTimeBlocksEndopints(this WebApplication app)
    {
        app.MapGet("api/user/time-blocks", GetTimeBlocks);
        app.MapPost("api/user/time-blocks", CreateTimeBlock);
        app.MapPut("api/user/time-blocks/{id:int}", UpdateTimeBlock);
        app.MapPut("api/user/time-blocks/update-order", UpdateOrder);
        app.MapDelete("api/user/time-blocks/{id:int}", DeleteTimeBlock);
    }

    private static async Task<IResult> DeleteTimeBlock(
        int id,
        HttpContext context,
        ITimeBlockService service)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.DeleteTimeBlock(id, userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateOrder(
        UpdateOrderRequest orderRequest,
        HttpContext context,
        ITimeBlockService service)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.UpdateTimeBlocksOrder(orderRequest, userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateTimeBlock(
        int id,
        TimeBlockRequest timeBlock,
        HttpContext context,
        ITimeBlockService service)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.UpdateTimeBlock(id, timeBlock, userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateTimeBlock(
        TimeBlockRequest timeBlock,
        HttpContext context,
        ITimeBlockService service)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.CreateTimeBlock(timeBlock, userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetTimeBlocks(
        HttpContext context,
        ITimeBlockService service)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.GetTimeBlocks(userId);
        return Results.Ok(result);
    }


}
