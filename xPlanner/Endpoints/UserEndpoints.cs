using xPlanner.Domain.Entities;
using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class UserEndpoits
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("api/user/profile", GetUser).RequireAuthorization();
        app.MapPut("api/user/profile", UpdateUser).RequireAuthorization();
    }

    private static async Task<IResult> GetUser(
        IUserService service,
        HttpContext context)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.GetUser(userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateUser(
        IUserService service,
        UserDto user,
        HttpContext context)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.UpdateUser(user, userId);
        return Results.Ok(result);
    }
}
