using Hangfire;
using NotificationService.Infrastructure;
using NotificationService.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this for API controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

// Add Application layer (business logic, validators)
builder.Services.AddApplication();

// Add Infrastructure layer (Hangfire, external services, logging)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHangfireDashboard();
app.UseHttpsRedirection();

// Add health check middleware
app.UseHealthChecks("/health");

// Add controller routing
app.MapControllers();

app.Run();


