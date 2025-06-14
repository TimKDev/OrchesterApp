FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrchesterApp.Api/OrchesterApp.Api.csproj", "OrchesterApp.Api/"]
COPY ["OrchesterApp.Application/OrchesterApp.Application.csproj", "OrchesterApp.Application/"]
COPY ["OrchesterApp.Contracts/OrchesterApp.Contracts.csproj", "OrchesterApp.Contracts/"]
COPY ["OrchesterApp.Domain/OrchesterApp.Domain.csproj", "OrchesterApp.Domain/"]
COPY ["OrchesterApp.Infrastructure/OrchesterApp.Infrastructure.csproj", "OrchesterApp.Infrastructure/"]
RUN dotnet restore "./OrchesterApp.Api/./OrchesterApp.Api.csproj"
COPY . .
WORKDIR "/src/OrchesterApp.Api"
RUN dotnet build "./OrchesterApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrchesterApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrchesterApp.Api.dll"]