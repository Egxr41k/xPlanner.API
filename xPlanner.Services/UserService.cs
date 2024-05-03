using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public class UserService
{
    private readonly IRepository<User> repository;
    private readonly IJwtProvider jwtProvider;

    public UserService(
        IRepository<User> userRepository, 
        IJwtProvider jwtProvider) 
    {
        repository = userRepository;
        this.jwtProvider = jwtProvider;
    }

    public async Task<User> GetUser(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var user = await repository.GetById(userId);

        return user ?? throw new UnauthorizedAccessException();
    }

    public async Task UpdateUser(HttpContext context, User user)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        user.Id = userId;

        await repository.Update(user);
    }
}
