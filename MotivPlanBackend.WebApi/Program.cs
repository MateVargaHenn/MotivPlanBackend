using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MotivPlanBackend.Application;
using MotivPlanBackend.Infrastructure;
using MotivPlanBackend.Persistence;
using MotivPlanBackend.Persistence.Extensions;
using MotivPlanBackend.WebApi;
using Serilog;

// Builder: .NET 10.0
var builder = WebApplication.CreateBuilder(args);

// Host
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddApplication().AddInfrastructure().AddPersistence(builder.Configuration).AddWebApi();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

await app.SeedDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "MotivPlan's Official Web API V1"));
}
else
{
    app.UseHttpsRedirection();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors("MyAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

await app.RunAsync();

namespace MotivPlanBackend.WebApi
{
    internal partial class Program;
}