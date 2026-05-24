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
        setControllers(services);
        addGoogleConfiguration(services);
        services.AddOpenApi();
        setCors(services);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        setApiVersioning(services);


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static void addGoogleConfiguration(IServiceCollection services)
    {
        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("Authentication__Google__ClientId")!;
                options.ClientSecret = Environment.GetEnvironmentVariable("Authentication__Google__ClientSecret")!;
                options.CallbackPath = "/signin-google";
            });
    }

    private static void setApiVersioning(IServiceCollection services)
    {
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
    }

    private static void setCors(IServiceCollection services)
    {
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
    }

    private static void setControllers(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelStateAttribute>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
    }
}