using System;
using ProReLe.Domain.Entities;

namespace ProReLe.Application.Interfaces.Queries
{
    public interface IProductQuery : IBaseQuery<Product>
    {
        IEnumerable<Product> GetByDescription(string description);
        IEnumerable<Product> GetAllOrderedByPrice();
        IEnumerable<Product> GetAllOrderedByAmount();
    }
}