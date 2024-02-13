using FinActions.Domain.AppSettings;
using FinActions.HttpApi.Host;
using FinActions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppSettingsOptions();

var connectionStrings = builder
                        .Configuration
                        .GetRequiredSection(nameof(ConnectionStrings))
                        .Get<ConnectionStrings>()!;
builder.Services
    .Configure<JwtOptions>(builder.Configuration.GetRequiredSection(nameof(JwtOptions)))
    .Configure<ConnectionStrings>(builder.Configuration.GetRequiredSection(nameof(ConnectionStrings)))
    .AddInfrastructureServices(connectionStrings)
    .AddApplicationServices()
    .AddAuthenticationServices()
    .AddSwaggerServices()
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddControllers();

var application = builder.Build();
// Configure the HTTP request pipeline.
application
    .UseSwagger()
    .UseSwaggerUI();

application
    .UseHttpsRedirection();

application
    .UseAuthorization()
    .UseAuthentication();

application.MapControllers();

application.Run();
