using Microsoft.AspNetCore.Http;
using xPlanner.Auth;
using xPlanner.Domain.Entities;

namespace xPlanner.Services;

public record AuthRequest(string Email, string Password);
public record AuthResponce(string AccessToken, User User);

public interface IAuthService
{
    Task<AuthResponce> Login(AuthRequest request, HttpContext context);
    Task<bool> Logout(HttpContext context);
    Task<AuthResponce> RefreshAccessToken(HttpContext context);
    Task<AuthResponce> Register(AuthRequest request, HttpContext context);
}

public class AuthService : IAuthService
{
    private readonly IUserService userService;
    private readonly IJwtProvider jwtProvider;

    public AuthService(
        IUserService userService,
        IJwtProvider jwtProvider)
    {
        this.userService = userService;
        this.jwtProvider = jwtProvider;
    }

    public async Task<AuthResponce> Register(
        AuthRequest request, 
        HttpContext context)
    {
        var isUserExists = await userService.CheckIfUserExists(request.Email);
        if (isUserExists) throw new Exception();
        
        var user = await userService.CreateUser(request.Email, request.Password);

        var accessToken = GenerateAndSaveTokens(user.Id, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task<AuthResponce> Login(
        AuthRequest request, 
        HttpContext context)
    {
        var user = await userService.GetUserByEmailAndPassword(request.Email, request.Password);

        var accessToken = GenerateAndSaveTokens(user.Id, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task<AuthResponce> RefreshAccessToken(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "userId");
        var userId = Convert.ToInt32(userIdClaim?.Value);

        var user = await userService.GetById(userId);

        var accessToken = GenerateAndSaveTokens(userId, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task<bool> Logout(HttpContext context)
    {
        context.Response.Cookies.Delete("refreshToken");
        return true;
    }

    private string GenerateAndSaveTokens(int userId, HttpContext context)
    {
        var accessToken = jwtProvider.GenerateToken(userId);
        var refreshToken = jwtProvider.GenerateToken(userId, 168);

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        context.Response.Cookies.Append("refreshToken", refreshToken, options);

        return accessToken;
    }
}
