using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Queries
{
    public class SaleQuery : ISaleQuery
    {
        private readonly IQueryable<Sale> SaleQueryable;

        public SaleQuery(IUnitOfWork unitOfWork)
        {
            SaleQueryable = unitOfWork.SaleRepository.Queryable.AsNoTracking();
        }

        public Sale? GetById(int id)
        {
            var entity = SaleQueryable
                .Include(s => s.Product)
                .Include(s => s.Client)
                .FirstOrDefault(p => p.Id == id);

            return entity;
        }

        public IEnumerable<Sale> GetByProduct(int productId)
        {
            var entities = SaleQueryable
                .Include(s => s.Product)
                .Include(s => s.Client)
                .Where(e => e.ProductId == productId)
                .ToList();

            return entities;
        }

        public IEnumerable<Sale> GetByClient(int clientId)
        {
            var entities = SaleQueryable
                .Include(s => s.Product)
                .Include(s => s.Client)
                .Where(e => e.ClientId == clientId)
                .ToList();

            return entities;
        }

        public IEnumerable<Sale> GetByDate(DateTimeOffset date)
        {
            var entities = SaleQueryable
                .Include(s => s.Product)
                .Include(s => s.Client)
                .Where(e => 
                    e.Date.Year == date.Year 
                    && e.Date.Month == date.Month 
                    && e.Date.Day == date.Day)
                .ToList();

            return entities;
        }

        public IEnumerable<Sale> GetAll()
        {
            var entities = SaleQueryable
                .Include(s => s.Product)
                .Include(s => s.Client)
                .ToList();

            return entities;
        }
    }
}