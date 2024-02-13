using FinActions.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class UsuarioPapelEntityConfiguration : IEntityTypeConfiguration<UsuarioPapel>
{
    public void Configure(EntityTypeBuilder<UsuarioPapel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UsuarioId);
    }
}
