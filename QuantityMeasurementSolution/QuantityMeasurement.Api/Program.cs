using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.RepositoryLayer.Data;
using QuantityMeasurement.RepositoryLayer.Repositories;
using RepositoryLayer.Interfaces;
using QuantityMeasurement.Domain.Services;

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


var connectionString = builder.Configuration.GetConnectionString("QuantityMeasurementDb")
    ?? throw new InvalidOperationException("Connection string 'QuantityMeasurementDb' is missing.");

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementOrmRepository>();

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();