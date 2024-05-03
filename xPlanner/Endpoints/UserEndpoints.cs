using Microsoft.AspNetCore.Http.HttpResults;
using xPlanner.Domain.Entities;
using xPlanner.Services;

namespace xPlanner.Endpoints;

public static class UserEndpoits
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("api/user/profile", GetUsers);
        app.MapGet("api/user/profile/{id:int}", GetUsersById);
        app.MapPost("api/user/profile", CreateUser);
        app.MapPut("api/user/profile/{id:int}", UpdateUser);
        app.MapDelete("api/user/profile/{id:int}", DeleteUser);
    }

    private static async Task GetUsers(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> GetUsersById(
        UserService service,
        HttpContext context)
    {
        var result = await service.GetUser(context);
        return Results.Ok(result);
    }

    private static async Task CreateUser(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateUser(
        UserService service,
        User user,
        HttpContext context)
    {
        await service.UpdateUser(context, user);
        return Results.Ok();
    }

    private static async Task DeleteUser(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
