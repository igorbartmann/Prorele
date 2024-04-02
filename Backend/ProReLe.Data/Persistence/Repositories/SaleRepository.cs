using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Data.Persistence.Repositories
{
    public class SaleRepository: BaseRepository<Sale>, ISaleRepository
    {
        private readonly ProReLeContext _context;
        public SaleRepository(ProReLeContext context) : base(context)
        {
            _context = context;
        }

        public override Sale? GetById(int id)
        {
            var entity = _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Client)
                .FirstOrDefault(s => s.Id == id);
                
            return entity;
        }
    }
}