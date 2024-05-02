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

    private static async Task DeleteTimeBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task UpdateOrder(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateTimeBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task CreateTimeBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task GetTimeBlocks(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
