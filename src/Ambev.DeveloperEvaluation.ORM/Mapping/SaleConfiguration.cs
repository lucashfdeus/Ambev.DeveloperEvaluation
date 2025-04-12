using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                   .IsRequired();

            builder.Property(s => s.CustomerId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.CustomerName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.BranchId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.BranchName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(s => s.CreatedAt)
                   .IsRequired();

            builder.Property(s => s.UpdatedAt)
                   .IsRequired();

            builder.Ignore(s => s.NetTotalAmount);

            builder.Ignore(s => s.GrossTotalAmount);

            builder.HasMany<SaleItem>("_items")
                   .WithOne()
                   .HasForeignKey("SaleId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata
                   .FindNavigation(nameof(Sale.Items))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
