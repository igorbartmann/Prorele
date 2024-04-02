using System;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Domain.Interfaces.OuW
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository {get;}
        IClientRepository ClientRepository {get;}
        ISaleRepository SaleRepository {get;}

        void Commit();
    }
}