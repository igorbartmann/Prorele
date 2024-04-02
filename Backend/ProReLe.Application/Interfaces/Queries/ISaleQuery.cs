using System;
using ProReLe.Domain.Entities;

namespace ProReLe.Application.Interfaces.Queries
{
    public interface ISaleQuery : IBaseQuery<Sale>
    {
        IEnumerable<Sale> GetByProduct(int productId);
        IEnumerable<Sale> GetByClient(int clientId);
        IEnumerable<Sale> GetByDate(DateTimeOffset date);
    }
}