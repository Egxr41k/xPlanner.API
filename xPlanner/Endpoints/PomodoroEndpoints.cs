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

    private static async Task DeleteSession(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateSession(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateRound(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task CreateSession(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task GetTodaySession(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
