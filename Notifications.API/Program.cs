using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notifications.Infrastructure.Persistence.DbContext;
using Notifications.Core.Interfaces;
using Notifications.Infrastructure.Services;
using Notifications.Infrastructure.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// 🔹 DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5433;Database=notifications;Username=postgres;Password=postgres"));

// 🔹 Services
builder.Services.AddScoped<INotificationService, NotificationService>();

// 🔹 JWT
var jwtKey = "SUPER_SECRET_KEY_123456";

builder.Services.AddSingleton(new JwtService(jwtKey));
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// 🔹 Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// 🔹 Middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => "API is running");

app.Run();