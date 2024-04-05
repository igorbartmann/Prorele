using System.Reflection;
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
        private readonly IProductQuery _productQuery;
        private readonly IProductService _productService;    
        private readonly IClientQuery _clientQuery;
        private readonly IClientService _clientService;    
        private readonly ISaleQuery _saleQuery;
        private readonly ISaleService _saleService;
        
        public SaleTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ProReLeContext>()
                .UseSqlServer(GlobalValues.CONNECTION_STRING)
                .Options;

            var unitOfWork = new UnitOfWork(new ProReLeContext(dbContextOptions));
            _productQuery = new ProductQuery(unitOfWork);
            _productService = new ProductService(unitOfWork);
            _clientQuery = new ClientQuery(unitOfWork);
            _clientService = new ClientService(unitOfWork);
            _saleQuery = new SaleQuery(unitOfWork);
            _saleService = new SaleService(unitOfWork);
        }

        [Fact, Order(0)]
        public void PrepareDatabase()
        {        
            var productsByDescription = _productQuery.GetByDescription(GlobalValues.SALE_PRODUCT_DESCRIPTION);
            foreach (var productByDescription in productsByDescription)
            {
                _productService.Delete(productByDescription);
            }

            var product = new Product(GlobalValues.SALE_PRODUCT_DESCRIPTION, GlobalValues.SALE_PRODUCT_PRICE, GlobalValues.SALE_PRODUCT_AMOUNT);
            var responseProduct = _productService.Include(product);
            if (responseProduct.Success)
            {
                product = _productQuery.GetByDescription(GlobalValues.SALE_PRODUCT_DESCRIPTION).FirstOrDefault();
            }
            else
            {
                Assert.Fail("Unable to clean the database!");
                return;
            }

            var clientByCpf = _clientQuery.GetByCpf(GlobalValues.SALE_CLIENT_CPF);
            if (clientByCpf is not null)
            {
                _clientService.Delete(clientByCpf);
            }

            var client = new Client(GlobalValues.SALE_CLIENT_NAME, GlobalValues.SALE_CLIENT_CPF);
            var responseClient = _clientService.Include(client);
            if (responseClient.Success)
            {
                client = _clientQuery.GetByCpf(GlobalValues.SALE_CLIENT_CPF);
            }
            else
            {
                Assert.Fail("Unable to clean the database!");
                return;
            }

            if (product is null || client is null)
            {
                Assert.Fail("Unable to clean the database!");
            }
        }

        [Fact, Order(1)]
        public void IncludeSale()
        {
            // Arrange
            var product = GetScenarioProduct();
            if (product is null)
            {
                Assert.Fail("The product was not found.");
                return;
            }

            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var sale = new Sale(product.Id, client.Id, GlobalValues.SALE_AMOUNT, GlobalValues.SALE_DISCONT_BEFORE_EDIT);

            // Act
            var response = _saleService.Include(sale);
            
            // Assert
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(2)]
        public void EditSale()
        {
            // Arrange
            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var sales = _saleQuery.GetByClient(client.Id);

            if (sales.Count() != 1)
            {
                Assert.Fail("Only 1 result was expected.");
                return;
            }

            var sale = sales.First();
            sale.Discount = GlobalValues.SALE_DISCOUNT_AFTER_EDIT;

            // Act
            var response = _saleService.Update(sale);
            
            // Assert
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(3)]
        public void GetSaleById()
        {
            // Arrange
            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var salesByClient = _saleQuery.GetByClient(client.Id);
            
            if (salesByClient.Count() != 1)
            {
                Assert.Fail("Only 1 result was expected.");
                return;
            }

            var sale = salesByClient.First();

            // Act
            var saleById = _saleQuery.GetById(sale.Id);

            if (saleById is null)
            {
                Assert.Fail("The sale was not found.");
                return;
            }
            
            // Assert
            var salesAreTheSame = 
                sale.Id == saleById.Id
                && sale.ProductId == saleById.ProductId
                && sale.ClientId == saleById.ClientId
                && sale.Amount == saleById.Amount
                && sale.InitialPrice == saleById.InitialPrice
                && sale.Discount == saleById.Discount
                && sale.FinalPrice == saleById.FinalPrice
                && sale.Date.ToString("dd/MM/yyyy").Equals(saleById.Date.ToString("dd/MM/yyyy"));

            Assert.True(salesAreTheSame, "The sales are not the same.");
        }

        [Fact, Order(4)]
        public void GetSalesByProduct()
        {
            // Act
            var product = GetScenarioProduct();
            if (product is null)
            {
                Assert.Fail("The product was not found.");
                return;
            }

            var salesByProduct = _saleQuery.GetByProduct(product.Id);
            
            // Assert
            Assert.True(salesByProduct.Count() == 1, "Only 1 result was expected.");
        }

        
        [Fact, Order(5)]
        public void GetSalesByClient()
        {
            // Act
            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var salesByClient = _saleQuery.GetByClient(client.Id);
            
            // Assert
            Assert.True(salesByClient.Count() == 1, "Only 1 result was expected.");
        }

        [Fact, Order(6)]
        public void GetSalesByDate()
        {
            // Arrange
            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var salesByClient = _saleQuery.GetByClient(client.Id);

            if(salesByClient.Count() != 1)
            {
                Assert.Fail("Only 1 result was expected.");
                return;
            }

            var sale = salesByClient.First();

            // Act
            var currentDate = DateTimeOffset.UtcNow;
            var salesByDate = _saleQuery.GetByDate(currentDate);
            
            // Assert
            var saleInTheList = salesByDate
                .Any(s => 
                    s.Id == sale.Id
                    && s.ProductId == sale.ProductId
                    && s.ClientId == sale.ClientId
                    && s.Amount == sale.Amount
                    && s.InitialPrice == sale.InitialPrice
                    && s.Discount == sale.Discount
                    && s.FinalPrice == sale.FinalPrice
                    && s.Date.ToString("dd/MM/yyyy").Equals(sale.Date.ToString("dd/MM/yyyy")));

            Assert.True(saleInTheList, "The sale is not in the list.");
        }

        [Fact, Order(7)]
        public void GetAllSales()
        {   
            // Act         
            var sales = _saleQuery.GetAll();

            // Assert
            Assert.True(sales.Any(), "There are no products in the list.");
        }

        [Fact, Order(8)]
        public void DeleteSale()
        {
            // Arrange
            var client = GetScenarioClient();
            if (client is null)
            {
                Assert.Fail("The client was not found.");
                return;
            }

            var sales = _saleQuery.GetByClient(client.Id);

            if (sales.Count() != 1)
            {
                Assert.Fail("Only 1 result was expected.");
                return;
            }

            var sale = sales.First();

            // Act
            var response = _saleService.Delete(sale);
            
            // Assert
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        #region Helper Methods
        private Product? GetScenarioProduct()
        {
            var products = _productQuery.GetByDescription(GlobalValues.SALE_PRODUCT_DESCRIPTION);
            if (products.Count() != 1)
            {
                return null;
            }

            var product = products.First();
            return product;
        }

        private Client? GetScenarioClient()
        {
            var client = _clientQuery.GetByCpf(GlobalValues.SALE_CLIENT_CPF);
            return client;
        }
        #endregion
    }
}