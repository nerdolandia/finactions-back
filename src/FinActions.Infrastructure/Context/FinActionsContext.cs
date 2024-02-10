using FinActions.Domain.Usuario;
using FinActions.Domain.Usuario.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Context;

public class FinActionsContext : IdentityDbContext<Usuario, Role, string>
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

        modelBuilder.Entity<Usuario>()
            .ToTable("Usuarios");
        
        modelBuilder.Entity<Role>()
            .ToTable("Roles");
        
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .ToTable("UsuariosLogins");
        
        modelBuilder.Entity<IdentityUserRole<string>>()
            .ToTable("UsuariosRoles");
        
        modelBuilder.Entity<IdentityUserToken<string>>()
            .ToTable("UsuariosTokens");
        
        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .ToTable("RolesClaims");
        
        modelBuilder.Entity<IdentityUserClaim<string>>()
            .ToTable("UsuariosClaims");
    }
}
