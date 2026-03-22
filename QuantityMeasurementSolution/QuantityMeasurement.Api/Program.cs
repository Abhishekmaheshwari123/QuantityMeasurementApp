using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Repositories;
using QuantityMeasurement.Domain.Services;
// using QuantityMeasurement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IQuantityMeasurementRepository>(
    QuantityMeasurementCacheRepository.Instance);

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();