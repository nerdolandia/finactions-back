using FinActions.Domain.Categorias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class CategoriaEfConfig : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property(x => x.DataCriacao)
                .HasDefaultValueSql("NOW()")
                .IsRequired();

        builder.Property(x => x.Id)
                .IsRequired();

        builder.Property(x => x.Nome)
                .HasMaxLength(150)
                .IsRequired();

        builder.HasOne(x => x.User)
                .WithMany(x => x.Categorias)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
    }
}
