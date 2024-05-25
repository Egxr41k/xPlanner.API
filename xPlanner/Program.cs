using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using xPlanner.Auth;
using xPlanner.Data;
using xPlanner.Data.Repository;
using xPlanner.Domain.Entities;
using xPlanner.Endpoints;
using xPlanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Authentication 
var jwtSection = builder.Configuration.GetSection("JwtOptions");
builder.Services.Configure<JwtOptions>(jwtSection);

var jwtOptions = jwtSection.Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["refreshToken"];

                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "Postgres")));

builder.Services.AddScoped<IRepository<PomodoroSession>, PomodoroSessionRepository>();
builder.Services.AddScoped<IRepository<TimeBlock>, TimeBlockRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<UserTask>, UserTaskRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPomodoroService, PomodoroService>();
builder.Services.AddScoped<ITimeBlockService, TimeBlockService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseCors(policy => policy
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapPomodoroEndpoints();
app.MapTimeBlocksEndopints();
app.MapUserEndpoints();
app.MapUserTaskEndpoints();

app.Run();
