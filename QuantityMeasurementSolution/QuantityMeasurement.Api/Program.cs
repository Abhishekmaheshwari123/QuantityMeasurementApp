using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Api.Contracts;
using QuantityMeasurement.Api.Middleware;
using QuantityMeasurement.Domain.Services;
using RepositoryLayer.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Repositories;

// using QuantityMeasurement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        string message = string.Join(
            ", ",
            context
                .ModelState.Values.SelectMany(v => v.Errors)
                .Select(e =>
                    string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid request." : e.ErrorMessage
                )
        );

        var errorResponse = new ApiErrorResponse
        {
            Status = StatusCodes.Status400BadRequest,
            Error = "Validation Error",
            Message = string.IsNullOrWhiteSpace(message) ? "Request validation failed." : message,
            Path = context.HttpContext.Request.Path,
        };

        return new BadRequestObjectResult(errorResponse);
    };
});

builder.Services.AddSingleton<IQuantityMeasurementRepository>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string provider = configuration["Repository:Provider"] ?? "Cache";

    if (string.Equals(provider, "Database", StringComparison.OrdinalIgnoreCase))
    {
        string? connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? configuration["Repository:ConnectionString"];

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return new QuantityMeasurementDatabaseRepository(new DatabaseConfig(connectionString));
        }
    }

    return new QuantityMeasurementCacheRepository();
});

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
