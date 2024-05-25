using xPlanner.Domain.Entities;
using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class UserEndpoits
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("api/user/profile", GetUser).RequireAuthorization();
        app.MapGet("api/user/profile/{id:int}", GetUsersById);
        app.MapPost("api/user/profile", CreateUser);
        app.MapPut("api/user/profile/{id:int}", UpdateUser).RequireAuthorization();
        app.MapDelete("api/user/profile/{id:int}", DeleteUser).RequireAuthorization();
    }

    private static async Task<IResult> GetUser(
        IUserService service,
        HttpContext context)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.GetUser(userId);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUsersById(
        int id,
        IUserService service,
        HttpContext context)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.GetUser(userId);
        return Results.Ok(result);
    }

    private static async Task CreateUser(
        HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateUser(
        int id,
        IUserService service,
        UserRequest user,
        HttpContext context)
    {
        var userId = Helpers.GetUserIdFromContext(context);

        var result = await service.UpdateUser(user, userId);
        return Results.Ok(result);
    }

    private static async Task DeleteUser(
        int id,
        HttpContext context)
    {
        throw new NotImplementedException();
    }
}
