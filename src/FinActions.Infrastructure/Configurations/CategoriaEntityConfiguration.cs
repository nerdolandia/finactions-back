using FinActions.Domain.Categorias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class CategoriaEntityConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.Property(x => x.Nome)
            .IsRequired();

        builder.Property(x => x.Cor)
            .HasDefaultValue("#FFF")
            .IsRequired();
    }
}