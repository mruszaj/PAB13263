using Xunit;
using ApiRest.Models;
using ApiRest.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Tests.Services
{
    public class ProduktServiceTests
    {
        private readonly DbContextOptions<ApkDbContext> _dbContextOptions;

        public ProduktServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AddTestProduct()
        {
            
            using var context = new ApkDbContext(_dbContextOptions);
            var service = new ProduktService(context);
            var product = new Product { Name = "Test Product", Price = 123.45m };

            
            service.AddProduct(product);

            
            var added = context.Products.Find(product.Id);
            Assert.NotNull(added);
            Assert.Equal("Test Product", added.Name);
        }

       
        [Fact]
        public void DeleteProduct()
        {
            
            using var context = new ApkDbContext(_dbContextOptions);
            var service = new ProduktService(context);

            var product = new Product
            {
                Name = "Product deleted",
                Price = 59.99m
            };

            context.Products.Add(product);
            context.SaveChanges();

            
            service.RemoveProduct(product.Id);

            
            var deleted = context.Products.Find(product.Id);
            Assert.Null(deleted);
        }

    }
}
