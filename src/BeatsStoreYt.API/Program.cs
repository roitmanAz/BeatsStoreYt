using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Middleware;
using BeatsStoreYt.API.Services.Auth;
using BeatsStoreYt.API.Services.Admin;
using BeatsStoreYt.API.Services.Catalog;
using BeatsStoreYt.API.Services.Notifications;
using BeatsStoreYt.API.Services.Storage;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BeatsStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<NotificationOptions>(builder.Configuration.GetSection("Notifications"));
builder.Services.Configure<AzureBlobOptions>(builder.Configuration.GetSection("AzureBlob"));

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? string.Empty;
var jwtIssuer = jwtSection["Issuer"] ?? string.Empty;
var jwtAudience = jwtSection["Audience"] ?? string.Empty;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<PasswordHasher<BeatsStoreYt.API.Data.Features.Users.User>>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISecurityEventLogger, SecurityEventLogger>();
builder.Services.AddScoped<IEmailService, StubEmailService>();
builder.Services.AddScoped<ISmsService, StubSmsService>();
builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

// Register Catalog Services
builder.Services.AddScoped<IBeatService, BeatService>();
builder.Services.AddScoped<IStyleService, StyleService>();
builder.Services.AddScoped<IKeyboardModelService, KeyboardModelService>();
builder.Services.AddScoped<IBeatSetService, BeatSetService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BeatsStoreYt API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<UserActivityMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
