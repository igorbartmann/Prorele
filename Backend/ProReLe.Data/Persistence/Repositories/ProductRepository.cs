using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Data.Persistence.Repositories
{
    public class ProductRepository: BaseRepository<Product>, IProductRepository
    {
        private readonly ProReLeContext _context;

        public ProductRepository(ProReLeContext context) : base(context)
        {
            _context = context;    
        }

         public override Product? GetById(int id)
        {
            var entity = _context.Products
                .Where(p => !p.LogicallyExcluded)
                .FirstOrDefault(p => p.Id == id);

            return entity;
        }
    }
}