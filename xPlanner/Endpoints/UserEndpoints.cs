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

    private static async Task GetUsersById(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task CreateUser(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateUser(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task DeleteUser(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
