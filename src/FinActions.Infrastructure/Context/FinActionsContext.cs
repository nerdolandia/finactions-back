using FinActions.Domain.Base;
using FinActions.Domain.Categorias;
using FinActions.Domain.Usuario;
using FinActions.Domain.Usuario.Papel;
using FinActions.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Context;

public class FinActionsContext : DbContext
{
    public FinActionsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Papel> Papeis { get; set; }
    public DbSet<UsuarioPapel> UsuariosPapeis { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UsuarioEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PapelEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioPapelEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaEntityConfiguration());
    }
}
