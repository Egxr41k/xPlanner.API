using Microsoft.AspNetCore.Http;

namespace xPlanner.Auth
{
    public interface IAuthService
    {
        Task<AuthResponce> Login(AuthRequest request, HttpContext context);
        Task Logout(HttpContext context);
        Task<AuthResponce> RefreshAccessToken(HttpContext context);
        Task<AuthResponce> Register(AuthRequest request, HttpContext context);
    }
}