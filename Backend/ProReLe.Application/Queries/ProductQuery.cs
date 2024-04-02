using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly IQueryable<Product> ProductQueryable;

        public ProductQuery(IUnitOfWork unitOfWork)
        {
            ProductQueryable = unitOfWork.ProductRepository.Queryable.Where(p => !p.LogicallyExcluded).AsNoTracking();
        }

        public Product? GetById(int id)
        {
            var entity = ProductQueryable.FirstOrDefault(p => p.Id == id);
            return entity;
        }

        public IEnumerable<Product> GetByDescription(string description)
        {
            var entities = ProductQueryable
                .Where(p => p.Description.Contains(description))
                .ToList();

            return entities;
        }

        public IEnumerable<Product> GetAll()
        {
            var entities = ProductQueryable.ToList();
            return entities;
        }

        public IEnumerable<Product> GetAllOrderedByPrice()
        {
             var entities = ProductQueryable.OrderBy(e => e.Price).ToList();
            return entities;
        }

        public IEnumerable<Product> GetAllOrderedByAmount()
        {
            var entities = ProductQueryable
                .OrderBy(e => e.Amount)
                .ToList();

            return entities;
        }
    }
}