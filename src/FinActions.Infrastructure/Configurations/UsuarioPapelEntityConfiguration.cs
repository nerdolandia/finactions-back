using FinActions.Domain.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class UsuarioPapelEntityConfiguration : IEntityTypeConfiguration<UsuarioPapel>
{
    public void Configure(EntityTypeBuilder<UsuarioPapel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Papel)
            .WithMany(x => x.UsuariosPapeis);

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.UsuariosPapeis);
        
        builder.HasIndex(x => x.UsuarioId);
    }
}