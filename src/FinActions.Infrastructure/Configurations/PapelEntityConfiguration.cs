using FinActions.Domain.Usuario;
using FinActions.Domain.Usuario.Papel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class PapelEntityConfiguration : IEntityTypeConfiguration<Papel>
{
    public void Configure(EntityTypeBuilder<Papel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200);

        builder.HasMany(x => x.Usuarios)
            .WithMany(x => x.Papeis)
            .UsingEntity<UsuarioPapel>();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}