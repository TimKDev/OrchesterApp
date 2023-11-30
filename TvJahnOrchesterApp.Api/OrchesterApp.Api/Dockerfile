#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrchesterApp.Api/TvJahnOrchesterApp.Api.csproj", "OrchesterApp.Api/"]
COPY ["TvJahnOrchesterApp.Application/TvJahnOrchesterApp.Application.csproj", "TvJahnOrchesterApp.Application/"]
COPY ["TvJahnOrchesterApp.Contracts/TvJahnOrchesterApp.Contracts.csproj", "TvJahnOrchesterApp.Contracts/"]
COPY ["TvJahnOrchesterApp.Domain/TvJahnOrchesterApp.Domain.csproj", "TvJahnOrchesterApp.Domain/"]
COPY ["TvJahnOrchesterApp.Infrastructure/TvJahnOrchesterApp.Infrastructure.csproj", "TvJahnOrchesterApp.Infrastructure/"]
RUN dotnet restore "./OrchesterApp.Api/./TvJahnOrchesterApp.Api.csproj"
COPY . .
WORKDIR "/src/OrchesterApp.Api"
RUN dotnet build "./TvJahnOrchesterApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TvJahnOrchesterApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TvJahnOrchesterApp.Api.dll"]