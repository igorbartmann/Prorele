using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProReLe.Domain.Entities;

namespace ProReLe.Data.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        #region Constants
        public const int DESCRIPTION_MIN_LENGTH = 3;
        public const int DESCRIPTION_MAX_LENGTH = 50;
        #endregion

        public void  Configure(EntityTypeBuilder<Product> builder)
        {
            // Table.
            builder.ToTable("Product");
            builder.HasKey(p => p.Id);

            // Properties.
            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("Description")
                .HasMaxLength(DESCRIPTION_MAX_LENGTH)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnName("Price")
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnName("Amount")
                .IsRequired();

            builder.Property(p => p.LogicallyExcluded)
                .HasColumnName("LogicallyExcluded")
                .IsRequired();
        }
    }
}