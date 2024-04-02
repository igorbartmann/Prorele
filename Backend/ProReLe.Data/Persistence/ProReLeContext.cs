using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using ProReLe.Data.Persistence.Configurations;
using ProReLe.Domain.Entities;

namespace ProReLe.Data.Persistence
{
    public class ProReLeContext : DbContext
    {
        #region Constants
        private const string PRORELE_SCHEMA = "PRORELE";
        #endregion

        public ProReLeContext(DbContextOptions<ProReLeContext> options) : base(options) {}

        public DbSet<Product> Products {get;set;} = null!;
        public DbSet<Client> Clients {get;set;} = null!;
        public DbSet<Sale> Sales {get;set;} = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(PRORELE_SCHEMA);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
        }
    }
}