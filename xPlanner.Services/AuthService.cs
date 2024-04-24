using Microsoft.AspNetCore.Http;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Auth;

public record AuthRequest(string Email, string Password);
public record AuthResponce(string AccessToken, User User);

public class AuthService : IAuthService
{
    private readonly IPasswordHasher passwordHasher;
    private readonly UserRepository userRepository;
    private readonly IJwtProvider jwtProvider;

    public AuthService(
        IPasswordHasher passwordHasher,
        IRepository<User> userRepository,
        IJwtProvider jwtProvider)
    {
        this.passwordHasher = passwordHasher;
        this.userRepository = (UserRepository)userRepository;
        this.jwtProvider = jwtProvider;
    }

    public async Task<AuthResponce> Register(
        AuthRequest request, 
        HttpContext context)
    {
        await CheckIfUserExists(request.Email);

        var user = await CreateUser(request);

        var accessToken = GenerateAndSaveTokens(user, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task<AuthResponce> Login(
        AuthRequest request, 
        HttpContext context)
    {
        var user = await GetUserByEmailAndPassword(request);

        var accessToken = GenerateAndSaveTokens(user, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task<AuthResponce> RefreshAccessToken(HttpContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var userId = jwtProvider.GetInfoFromToken(refreshToken);

        var user = await userRepository.GetById(userId);

        var accessToken = GenerateAndSaveTokens(user, context);

        return new AuthResponce(accessToken, user);
    }

    public async Task Logout(HttpContext context)
    {
        context.Response.Cookies.Delete("refreshToken");
    }

    private async Task CheckIfUserExists(string email)
    {
        var existingUser = await userRepository.GetByEmail(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists.");
        }
    }

    private async Task<User> CreateUser(AuthRequest request)
    {
        var hashedPassword = passwordHasher.Generate(request.Password);

        var user = new User
        {
            Email = request.Email,
            Password = hashedPassword
        };

        await userRepository.Add(user);

        return user;
    }

    private async Task<User> GetUserByEmailAndPassword(AuthRequest authRequest)
    {
        var user = await userRepository.GetByEmail(authRequest.Email);

        if (user == null || !passwordHasher.Verify(authRequest.Password, user.Password))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        return user;
    }

    private string GenerateAndSaveTokens(User user, HttpContext context)
    {
        var accessToken = jwtProvider.GenerateToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        context.Response.Cookies.Append("refreshToken", refreshToken, options);

        return accessToken;
    }
}
