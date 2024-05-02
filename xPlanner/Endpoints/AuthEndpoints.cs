using xPlanner.Auth;
using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("api/auth/register", RegisterHandler);
        app.MapPost("api/auth/login", LoginHandler);
        app.MapPost("api/auth/access-token", AccessTokenHandler);
        app.MapPost("api/auth/logout", LogoutHandler);
    }

    private static async Task<IResult> RegisterHandler(
        AuthRequest request, 
        AuthService service, 
        HttpContext context)
    {
        var result = await service.Register(request, context);
        return Results.Ok(result);
    }

    private static async Task<IResult> LoginHandler(
        AuthRequest request, 
        AuthService service, 
        HttpContext context)
    {
        var result = await service.Login(request, context);
        return Results.Ok(result);
    }
    private static async Task<IResult> AccessTokenHandler(
        AuthService service,
        HttpContext context)
    {
        var result = await service.RefreshAccessToken(context);
        return Results.Ok(result);
    }

    private static async Task<IResult> LogoutHandler(
        AuthService service, 
        HttpContext context)
    {
        await service.Logout(context);
        return Results.Ok();
    }
}
