using Asp.Versioning;
using MotivPlanBackend.Application.Attributes;
using MotivPlanBackend.WebApi.Handlers;
using System.Text.Json;

namespace MotivPlanBackend.WebApi;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelStateAttribute>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        services.AddOpenApi();
        services.AddCors(options =>
        {
            options.AddPolicy("MyAllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://motivplan-frontend.azurewebsites.net")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        })
        .AddApiExplorer(options => options.GroupNameFormat = "'v'V");


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}