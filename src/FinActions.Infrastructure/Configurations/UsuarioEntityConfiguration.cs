using FinActions.Domain.Usuarios;
using FinActions.Domain.Usuarios.Papeis;
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
            .HasMaxLength(UsuarioConsts.LimiteCampoEmail);

        builder.Property(x => x.Salt)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(x => x.Papeis)
            .WithMany(x => x.Usuarios)
            .UsingEntity<UsuarioPapel>();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}