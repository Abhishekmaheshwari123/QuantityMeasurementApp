using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurement.RepositoryLayer.Data;
using QuantityMeasurement.RepositoryLayer.Repositories;
using RepositoryLayer.Interfaces;
using QuantityMeasurement.Domain.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


var connectionString = builder.Configuration.GetConnectionString("QuantityMeasurementDb")
    ?? throw new InvalidOperationException("Connection string 'QuantityMeasurementDb' is missing.");

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementOrmRepository>();
builder.Services.AddScoped<IUserAuthRepository, UserAuthRepository>();

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Read JWT values once during startup and use them for token validation setup.
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "QuantityMeasurement.Api";
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? "QuantityMeasurement.Client";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            // Symmetric key must match key used when issuing tokens in AuthController.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseForwardedHeaders();
app.UseRouting();

// Apply pending migrations on startup so schema stays aligned with the current model.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    // dbContext.Database.Migrate();
}

app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();
// Validate incoming bearer tokens before authorization checks.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();