# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["MotivPlanBackend.WebApi/MotivPlanBackend.WebApi.csproj", "MotivPlanBackend.WebApi/"]
COPY ["MotivPlanBackend.Application/MotivPlanBackend.Application.csproj", "MotivPlanBackend.Application/"]
COPY ["MotivPlanBackend.Domain/MotivPlanBackend.Domain.csproj", "MotivPlanBackend.Domain/"]
COPY ["MotivPlanBackend.Shared/MotivPlanBackend.Shared.csproj", "MotivPlanBackend.Shared/"]
COPY ["MotivPlanBackend.Infrastructure/MotivPlanBackend.Infrastructure.csproj", "MotivPlanBackend.Infrastructure/"]
COPY ["MotivPlanBackend.Persistence/MotivPlanBackend.Persistence.csproj", "MotivPlanBackend.Persistence/"]
RUN dotnet restore "./MotivPlanBackend.WebApi/MotivPlanBackend.WebApi.csproj"
COPY . .
WORKDIR "/src/MotivPlanBackend.WebApi"
RUN dotnet build "./MotivPlanBackend.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MotivPlanBackend.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MotivPlanBackend.WebApi.dll"]