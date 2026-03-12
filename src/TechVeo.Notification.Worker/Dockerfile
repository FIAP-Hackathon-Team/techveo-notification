ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

FROM ${DOTNET_RUNTIME} AS base
RUN apt-get update && \
    apt-get install -y --no-install-recommends ffmpeg wget && \
    rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 8080

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN
ARG GITHUB_ACTOR
WORKDIR /src
COPY ["src/TechVeo.Notification.Worker/TechVeo.Notification.Worker.csproj", "TechVeo.Notification.Worker/"]
COPY ["src/TechVeo.Notification.Application/TechVeo.Notification.Application.csproj", "TechVeo.Notification.Application/"]
COPY ["src/TechVeo.Notification.Domain/TechVeo.Notification.Domain.csproj", "TechVeo.Notification.Domain/"]
COPY ["src/TechVeo.Notification.Contracts/TechVeo.Notification.Contracts.csproj", "TechVeo.Notification.Contracts/"]
COPY ["src/TechVeo.Notification.Infra/TechVeo.Notification.Infra.csproj", "TechVeo.Notification.Infra/"]
RUN dotnet nuget add source --username "${GITHUB_ACTOR}" --password "${GITHUB_TOKEN}" --store-password-in-clear-text --name github "https://nuget.pkg.github.com/FIAP-Hackathon-Team/index.json"
RUN dotnet restore "./TechVeo.Notification.Worker/TechVeo.Notification.Worker.csproj"
COPY src/ .
WORKDIR /src/TechVeo.Notification.Worker
RUN dotnet build "./TechVeo.Notification.Worker.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechVeo.Notification.Worker.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechVeo.Notification.Worker.dll"]
