using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using xPlanner.Domain.Entities;

namespace xPlanner.Auth;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiredHours { get; set; }
}

public interface IJwtProvider
{
    string GenerateToken(User user);
}

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        this.options = options.Value;
    }

    public string GenerateToken(User user)
    {
        Claim userId = new("userId:", user.Id.ToString());

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: [userId],
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(options.ExpiredHours));

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
