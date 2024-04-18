using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Endpoints;

public static class UserEndpoits
{
    public static void MapUserEndpoints(this WebApplication app)
    {

        app.MapGet("/api/user", GetAllNotesHandler);
        app.MapGet("/api/user/{id:int}", GetNoteByIdHandler);
        app.MapPost("/api/user", AddNoteHandler);
        app.MapPut("/api/user", UpdateNoteHandler);
        app.MapDelete("/api/user/{id:int}", DeleteNoteHandler);
    }

    private static async Task<IResult> GetAllNotesHandler(IRepository<User> repository)
    {
        var result = await repository.GetAll();
        return Results.Ok(result);
    }

    private static async Task<IResult> GetNoteByIdHandler(IRepository<User> repository, int id)
    {
        var note = await repository.GetById(id);
        return note.Id == 0 ? Results.NotFound() : Results.Ok(note);
    }

    private static async Task<IResult> AddNoteHandler(IRepository<User> repository, User user)
    {
        await repository.Add(user);
        return Results.CreatedAtRoute("/api/user/{id}", new { id = user.Id }, user);
    }

    private static async Task<IResult> UpdateNoteHandler(IRepository<User> repository, User user)
    {
        var existingUser = await repository.GetById(user.Id);
        if (existingUser == null) return Results.NotFound();

        existingUser.Name = user.Name;

        await repository.Update(existingUser);
        return Results.Ok(existingUser);
    }

    private static async Task<IResult> DeleteNoteHandler(IRepository<User> repository, int id)
    {
        var existingUser = await repository.GetById(id);
        if (existingUser == null) return Results.NotFound();

        await repository.Delete(existingUser.Id);
        return Results.Ok();
    }
}
