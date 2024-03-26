using FinActions.Domain.AppSettings;
using FinActions.HttpApi.Host;
using FinActions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppSettingsOptions();

var connectionStrings = builder
                        .Configuration
                        .GetRequiredSection(nameof(ConnectionStrings))
                        .Get<ConnectionStrings>()!;
var jwtOptions = builder
                    .Configuration
                    .GetRequiredSection(nameof(JwtOptions))
                    .Get<JwtOptions>()!;
builder.Services
    .Configure<JwtOptions>(x => x = jwtOptions)
    .Configure<ConnectionStrings>(x => x = connectionStrings)
    .AddApplicationServices(connectionStrings)
    .AddAuthenticationServices(jwtOptions)
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
    .UseAuthentication()
    .UseAuthorization();

application.MapControllers();

application.Run();
