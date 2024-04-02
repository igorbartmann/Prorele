using System;
using ProReLe.Data.Persistence.Repositories;
using ProReLe.Domain.Interfaces.OuW;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Data.Persistence.OuW
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Dependencies
        private readonly ProReLeContext _context;
        private IProductRepository _productRepository = null!;
        private IClientRepository _clientRepository = null!;
        private ISaleRepository _saleRepository = null!;
        #endregion

        public UnitOfWork(ProReLeContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository 
        {
            get
            {
                _productRepository ??= new ProductRepository(_context);
                return _productRepository;
            }
        }

        public IClientRepository ClientRepository 
        { 
            get
            {
                _clientRepository ??= new ClientRepository(_context);
                return _clientRepository;
            }
        }

        public ISaleRepository SaleRepository 
        {
            get
            {
                _saleRepository ??= new SaleRepository(_context);
                return _saleRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}