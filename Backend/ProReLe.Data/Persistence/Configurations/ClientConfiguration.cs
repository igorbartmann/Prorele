using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProReLe.Domain.Entities;

namespace ProReLe.Data.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        #region Constants
        public const int NAME_MIN_LENGTH = 3;
        public const int NAME_MAX_LENGTH = 50;
        public const int CPF_LENGTH = 11;
        #endregion

        public void  Configure(EntityTypeBuilder<Client> builder)
        {
            // Table.
            builder.ToTable("Client");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Cpf).IsUnique();

            // Properties.
            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasMaxLength(NAME_MAX_LENGTH)
                .IsRequired();

            builder.Property(p => p.Cpf)
                .HasColumnName("Cpf")
                .HasMaxLength(CPF_LENGTH)
                .IsRequired();

            builder.Property(p => p.LogicallyExcluded)
                .HasColumnName("LogicallyExcluded")
                .IsRequired();
        }
    }
}