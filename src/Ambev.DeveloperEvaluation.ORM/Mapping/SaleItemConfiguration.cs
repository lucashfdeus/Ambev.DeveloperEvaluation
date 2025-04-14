using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            builder.HasOne(i => i.Sale)
             .WithMany(s => s.Items)
             .HasForeignKey(i => i.SaleId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.ProductId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(i => i.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.IsCancelled)
                .HasDefaultValue(false);

            builder.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.GrossTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.NetTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(i => i.SaleId);
            builder.HasIndex(i => i.ProductId);
        }
    }
}