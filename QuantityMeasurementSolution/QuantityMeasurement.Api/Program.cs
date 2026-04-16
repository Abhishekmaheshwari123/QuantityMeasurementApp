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

// -------------------- SERVICES --------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS POLICY
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://quantitymeasurementapp-frontend-po29.onrender.com",
                "https://quantitymeasurementapp-frontend-po29.onrender.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // .AllowCredentials(); // enable only if using cookies
    });
});

// ✅ FIX FOR RENDER PROXY
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// DB
var connectionString = builder.Configuration.GetConnectionString("QuantityMeasurementDb")
    ?? throw new InvalidOperationException("Connection string missing");

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(connectionString));

// DI
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementOrmRepository>();
builder.Services.AddScoped<IUserAuthRepository, UserAuthRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key missing");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// -------------------- APP --------------------

var app = builder.Build();

// Ensure schema is up to date in deployed environments before serving requests.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    dbContext.Database.Migrate();
}

// ✅ IMPORTANT: HANDLE RENDER HEADERS
app.UseForwardedHeaders();

// ✅ HANDLE PREFLIGHT (OPTIONS)
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }
    await next();
});

// ✅ APPLY CORS (VERY IMPORTANT POSITION)
app.UseCors("FrontendPolicy");

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();