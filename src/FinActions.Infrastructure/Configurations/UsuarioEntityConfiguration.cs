using FinActions.Domain.Usuario;
using FinActions.Domain.Usuario.Papel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(x => x.Papeis)
            .WithMany(x => x.Usuarios)
            .UsingEntity<UsuarioPapel>();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}