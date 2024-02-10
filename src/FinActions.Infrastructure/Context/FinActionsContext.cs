using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Context;

public class FinActionsContext : DbContext
{
    public FinActionsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}