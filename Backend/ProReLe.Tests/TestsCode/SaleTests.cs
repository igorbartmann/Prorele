using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Queries;
using ProReLe.Application.Services;
using ProReLe.Data.Persistence;
using ProReLe.Data.Persistence.OuW;
using ProReLe.Domain.Entities;
using Xunit.Extensions.Ordering;

namespace ProReLe.Tests
{
    [Order(3)]
    public class SaleTests
    {
        private readonly ISaleQuery _saleQuere;
        private readonly ISaleService _saleService;        
        
        public SaleTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ProReLeContext>()
                .UseSqlServer(GlobalValues.CONNECTION_STRING)
                .Options;

            var unitOfWork = new UnitOfWork(new ProReLeContext(dbContextOptions));
            _saleQuere = new SaleQuery(unitOfWork);
            _saleService = new SaleService(unitOfWork);
        }

        [Fact, Order(0)]
        public void PrepareDatabase()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(1)]
        public void IncludeSale()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(2)]
        public void EditSale()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(3)]
        public void DeleteSale()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(4)]
        public void GetSaleById()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(5)]
        public void GetSalesByProduct()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        
        [Fact, Order(6)]
        public void GetSalesByClient()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(7)]
        public void GetSalesByDate()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(8)]
        public void GetAllSales()
        {            
            throw new NotImplementedException("Test case not implemented!");
        }
    }
}