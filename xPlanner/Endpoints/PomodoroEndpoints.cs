using xPlanner.Domain.Entities;
using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class PomodoroEndpoints
{
    public static void MapPomodoroEndpoints(this WebApplication app)
    {
        app.MapGet("api/user/timer/today", GetTodaySession);
        app.MapPost("api/user/timer", CreateSession);
        app.MapPut("api/user/timer/round/{id:int}", UpdateRound);
        app.MapPut("api/user/timer/{id:int}", UpdateSession);
        app.MapDelete("api/user/timer/{id:int}", DeleteSession);
    }

    private static async Task<IResult> DeleteSession(
        int id,
        HttpContext context,
        PomodoroService service)
    {
        var result = await service.DeleteSession(context, id);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateSession(
        int id,
        HttpContext context,
        PomodoroService service,
        PomodoroSession session)
    {
        var result = await service.UpdateSession(context, session);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateRound(
        int id,
        HttpContext context, 
        PomodoroService service,
        PomodoroRoundRequest round)
    {
        var result = await service.UpdateRound(id, context, round);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateSession(
        HttpContext context,
        PomodoroService service)
    {
        var result = await service.CreateSession(context);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetTodaySession(
        HttpContext context,
        PomodoroService service)
    {
        var result = await service.GetTodaySession(context);
        return Results.Ok(result);
    }
}
