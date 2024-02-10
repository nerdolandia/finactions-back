using FinActions.Domain.Usuario;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Context;

public class FinActionsContext : DbContext
{
    public FinActionsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Usuario>()
            .Property(x => x.Nome)
            .HasMaxLength(50);
    }
}