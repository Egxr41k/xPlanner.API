using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;

namespace xPlanner.Auth;

public record AuthRequest(string Email, string Password);

public class AuthService
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

    public async Task Register(AuthRequest authRequest)
    {
        var hashedPassword = passwordHasher.Generate(authRequest.Password);

        User user = new()
        {
            Email = authRequest.Email,
            Password = hashedPassword,
        };

        await userRepository.Add(user);
    }

    public async Task<string> Login(AuthRequest authRequest)
    {
        User user = await userRepository.GetByEmail(authRequest.Email);

        bool isVerified = passwordHasher.Verify(authRequest.Password, user.Password);

        if (!isVerified)
        {
            throw new ArgumentException();
        }

        string token = jwtProvider.GenerateToken(user);
        return token;
    }
}
