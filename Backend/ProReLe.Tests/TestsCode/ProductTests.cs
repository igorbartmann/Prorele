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
    [Order(1)]
    public class ProductTests
    {
        private readonly IProductQuery _productQuere;
        private readonly IProductService _productService;        
        
        public ProductTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ProReLeContext>()
                .UseSqlServer(GlobalValues.CONNECTION_STRING)
                .Options;

            var unitOfWork = new UnitOfWork(new ProReLeContext(dbContextOptions));
            _productQuere = new ProductQuery(unitOfWork);
            _productService = new ProductService(unitOfWork);
        }

        [Fact, Order(0)]
        public void PrepareDatabase()
        {
            var productsCreated = _productQuere.GetByDescription(GlobalValues.PRODUCT_DESCRIPTION_BEFORE_EDIT);
            var productsEdited = _productQuere.GetByDescription(GlobalValues.PRODUCT_DESCRIPTION_AFTER_EDIT);
            var products = productsCreated.Concat(productsEdited);

            foreach(var product in products)
            {
                var response = _productService.Delete(product);
                if (!response.Success)
                {
                    Assert.Fail("Unable to clean the database!");
                }
            }
        }

        [Fact, Order(1)]
        public void IncludeProduct()
        {
            // Arrange: variables declarations and initilizations.
            var product = new Product(
                GlobalValues.PRODUCT_DESCRIPTION_BEFORE_EDIT, 
                GlobalValues.PRODUCT_PRICE_BEFORE_EDIT, 
                GlobalValues.PRODUCT_AMOUNT_BEFORE_EDIT);
            
            // Act: call to the method to be tested.
            var response = _productService.Include(product);
            
            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(2)]
        public void EditProduct()
        {
            // Arrange: variables declarations and initilizations.
            var productsByDescription = _productQuere.GetByDescription(GlobalValues.PRODUCT_DESCRIPTION_BEFORE_EDIT);

            var product = productsByDescription
                .FirstOrDefault(p => 
                    p.Description == GlobalValues.PRODUCT_DESCRIPTION_BEFORE_EDIT 
                    && p.Price == GlobalValues.PRODUCT_PRICE_BEFORE_EDIT 
                    && p.Amount == GlobalValues.PRODUCT_AMOUNT_BEFORE_EDIT);
            
            if (product is null)
            {
                Assert.Fail("The product was not found!");
            }

            // Act: call to the method to be tested.
            product.Description = GlobalValues.PRODUCT_DESCRIPTION_AFTER_EDIT;
            product.Price = GlobalValues.PRODUCT_PRICE_AFTER_EDIT;
            product.Amount = GlobalValues.PRODUCT_AMOUNT_AFTER_EDIT;

            var response = _productService.Update(product);
            
            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(3)]
        public void DeleteProduct()
        {
            // Arrange: variables declarations and initilizations.
            var productsByDescription = _productQuere.GetByDescription(GlobalValues.PRODUCT_DESCRIPTION_AFTER_EDIT);
            var product = productsByDescription
                .FirstOrDefault(p => 
                    p.Description == GlobalValues.PRODUCT_DESCRIPTION_AFTER_EDIT 
                    && p.Price == GlobalValues.PRODUCT_PRICE_AFTER_EDIT 
                    && p.Amount == GlobalValues.PRODUCT_AMOUNT_AFTER_EDIT);

            if (product is null)
            {
                Assert.Fail("The product was not found!");
            }
            
            // Act: call to the method to be tested.
            var response = _productService.Delete(product);
            
            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(4)]
        public void GetProductById()
        {
            // Arrange: variables declarations and initilizations.
            var products = _productQuere.GetAll();

            var productIndex = products.Count() / 2;
            var product = products.ElementAt(productIndex);
            
            // Act: call to the method to be tested.
            var productById = _productQuere.GetById(product.Id);

            if (productById is null)
            {
                Assert.Fail("The product was not found!");
            }
            
            // Assert: result validation
            var productAreTheSame = 
                product.Id == productById.Id 
                && product.Description == productById.Description
                && product.Price == productById.Price 
                && product.Amount == productById.Amount;

            Assert.True(productAreTheSame, "The product found is not the same of the query filter.");
        }

        [Fact, Order(5)]
        public void GetProductsByDescription()
        {
            // Arrange: variables declarations and initilizations.
            var products = _productQuere.GetAll();
            
            var productIndex = products.Count() / 2;
            var product = products.ElementAt(productIndex);
            
            // Act: call to the method to be tested.
            var productsByDescription = _productQuere.GetByDescription(product.Description);

            if (!productsByDescription.Any())
            {
                Assert.Fail("The product was not found!");
            }
            
            // Assert: result validation
            var productAreInTheList = productsByDescription
                .Any(p => p.Id == product.Id 
                    && p.Description == product.Description 
                    && p.Price == product.Price 
                    && p.Amount == product.Amount);

            Assert.True(productAreInTheList, "The product is not in the list.");
        }

        [Fact, Order(6)]
        public void GetAllProductsOrderedByPrice()
        {
            // Arrange: variables declarations and initilizations.
            var products = _productQuere.GetAllOrderedByPrice();
            
            // Act: call to the method to be tested.
            var success = true;
            decimal currentPrice = products.FirstOrDefault()?.Price ?? 0;
            foreach(var product in products)
            {
                if (product.Price >= currentPrice)
                {
                    currentPrice = product.Price;
                }
                else
                {
                    success = false;
                }
            }

            // Assert: result validation
            Assert.True(success, "The user list is not ordered by price.");
        }

        [Fact, Order(7)]
        public void GetAllProductsOrderedByAmount()
        {
            // Arrange: variables declarations and initilizations.
            var products = _productQuere.GetAllOrderedByAmount();
            
            // Act: call to the method to be tested.
            var success = true;
            decimal currentAmount = products.FirstOrDefault()?.Amount ?? 0;
            foreach(var product in products)
            {
                if (product.Amount >= currentAmount)
                {
                    currentAmount = product.Amount;
                }
                else
                {
                    success = false;
                }
            }

            // Assert: result validation
            Assert.True(success, "The user list is not ordered by amount.");
        }

        [Fact, Order(8)]
        public void GetAllProducts()
        {            
            // Act: call to the method to be tested.
            var products = _productQuere.GetAll();

            // Assert: result validation
            Assert.True(products.Any(), "There are no products in the list.");
        }
    }
}