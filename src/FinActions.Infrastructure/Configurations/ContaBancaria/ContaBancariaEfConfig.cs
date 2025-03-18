using FinActions.Domain.ContasBancarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class ContaBancariaEfConfig : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.ToTable("ContasBancarias");

        builder.Property(x => x.DataCriacao)
                .HasDefaultValueSql("NOW()");

        builder.Property(x => x.Nome)
                .HasMaxLength(150)
                .IsRequired();

        builder.Property(x => x.Saldo)
                .HasColumnType("money")
                .IsRequired();

        builder.Property(x => x.TipoConta)
                .IsRequired();

        builder.HasOne(x => x.User)
                .WithMany(x => x.ContasBancarias)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
    }
}
