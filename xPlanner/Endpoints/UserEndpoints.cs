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
        UserService service,
        HttpContext context)
    {
        var result = await service.GetUser(context);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUsersById(
        int id,
        UserService service,
        HttpContext context)
    {
        var result = await service.GetUser(context);
        return Results.Ok(result);
    }

    private static async Task CreateUser(
        HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateUser(
        int id,
        UserService service,
        UserRequest user,
        HttpContext context)
    {
        var result = await service.UpdateUser(context, user);
        return Results.Ok(result);
    }

    private static async Task DeleteUser(
        int id,
        HttpContext context)
    {
        throw new NotImplementedException();
    }
}
