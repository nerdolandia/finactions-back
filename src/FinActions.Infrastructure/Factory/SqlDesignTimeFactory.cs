
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinActions.Infrastructure.Factory;

public class SqlDesignTimeFactory : IDesignTimeDbContextFactory<FinActionsContext>
{
    public FinActionsContext CreateDbContext(string[] args)
    {

        IConfiguration configuration = new ConfigurationBuilder()
                                        .SetBasePath(Environment.CurrentDirectory)
                                        .AddJsonFile("appsettings.json")
                                        .Build();

        var optionsBuilder = new DbContextOptionsBuilder()
                            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new FinActionsContext(optionsBuilder.Options);
    }
}