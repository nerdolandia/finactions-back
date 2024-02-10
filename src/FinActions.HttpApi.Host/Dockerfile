FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build and publish a release
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/FinActions.Domain/FinActions.Domain.csproj", "src/FinActions.Domain/"]
COPY ["src/FinActions.Contracts/FinActions.Contracts.csproj", "src/FinActions.Contracts/"]
COPY ["src/FinActions.Infrastructure/FinActions.Infrastructure.csproj", "src/FinActions.Infrastructure/"]
COPY ["src/FinActions.Application/FinActions.Application.csproj", "src/FinActions.Application/"]
COPY ["src/FinActions.HttpApi.Host/FinActions.HttpApi.Host.csproj", "src/FinActions.HttpApi.Host/"]
RUN dotnet restore "src/FinActions.HttpApi.Host/FinActions.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/FinActions.HttpApi.Host"
RUN dotnet build "FinActions.HttpApi.Host.csproj" -c Release -o /app/build

# Build and publish a release
FROM build AS publish
RUN dotnet publish "FinActions.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinActions.HttpApi.Host.dll"]