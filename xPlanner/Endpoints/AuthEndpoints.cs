using xPlanner.Auth;

namespace xPlanner.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("register", RegisterHandler);
        app.MapPost("login", LoginHandler);
        app.MapPost("logout", LogoutHandler);
    }

    private static async Task<IResult> RegisterHandler(AuthRequest request, AuthService service)
    {
        await service.Register(request);
        return Results.Ok();

    }

    private static async Task<IResult> LoginHandler(AuthRequest request, AuthService service, HttpContext context)
    {
        string token = await service.Login(request);

        context.Response.Cookies.Append("tasty-cookies", token);

        return Results.Ok();
    }

    private static async Task<IResult> LogoutHandler()
    {
        return Results.Ok();
    }
}
