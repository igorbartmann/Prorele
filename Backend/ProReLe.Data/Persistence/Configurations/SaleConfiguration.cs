using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProReLe.Domain.Entities;

namespace ProReLe.Data.Persistence.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void  Configure(EntityTypeBuilder<Sale> builder)
        {
            // Table.
            builder.ToTable("Sale");
            builder.HasKey(p => p.Id);

            // Properties.
            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(p => p.ProductId)
                .HasColumnName("ProductId")
                .IsRequired();

            builder.Property(p => p.ClientId)
                .HasColumnName("ClientId")
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnName("Amount")
                .IsRequired();

             builder.Property(p => p.InitialPrice)
                .HasColumnName("InitialPrice")
                .IsRequired();

            builder.Property(p => p.Discount)
                .HasColumnName("Discount");

            builder.Property(p => p.FinalPrice)
                .HasColumnName("FinalPrice")
                .IsRequired();

            builder.Property(p => p.Date)
                .HasColumnName("Date")
                .IsRequired();

            // Relations
            builder.HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .IsRequired();

            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .IsRequired();  
        }
    }
}