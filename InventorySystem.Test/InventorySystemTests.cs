using InventorySystem.Core.Data;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace InventorySystem.Test
{
    [TestClass]
    public sealed class InventorySystemTests
    {
        //Testing InventoryServices

        private Category _category = null!;
        private InventoryService _inventoryService = null!;
        private IDbContextFactory<InventoryDbContext> _factory = null!;
        private Product _product = null!;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _factory = new PooledDbContextFactory<InventoryDbContext>(options);

            var _activityService = new ActivityService();
            _inventoryService = new InventoryService(_factory, _activityService);
           

           await using var context = await _factory.CreateDbContextAsync();

            _category = new Category("Test Category");
            context.Categories.Add(_category);
           await context.SaveChangesAsync();
          
            _product = new Product("Test Product", _category, 10.0m, 0, 2);
           await _inventoryService.AddProductAsync(_product);
        }

        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(10, 5, 15)]
        public async Task AddStockToProduct_IncreasesQuantity(int productQuantity, int addedQuantity, int expectedQuantity)
        {
            // Arrange
            await using var context = await _factory.CreateDbContextAsync();

            var product = await context.Products
                .FirstAsync(p => p.ProductId == _product.ProductId);

            product.Quantity = productQuantity;
            await context.SaveChangesAsync();


            // Act
            await _inventoryService.AddStockAsync(product, addedQuantity);
            // Assert
            await context.Entry(product).ReloadAsync(); // Reload the product from the database to get the updated quantity
            Assert.AreEqual(expectedQuantity, product.Quantity);
        }
        [TestMethod]
        [DataRow(8, 3, 5)]
        [DataRow(15, 5, 10)]
        public async Task RemoveStockFromProduct_DecreasesQuantity(int productQuantity, int removedQuantity, int expectedQuantity)
        {
            // Arrange
            await using var context = await _factory.CreateDbContextAsync();

            var product = await context.Products
                .FirstAsync(p => p.ProductId == _product.ProductId);

            product.Quantity = productQuantity;
            await context.SaveChangesAsync();
            // Act
            await _inventoryService.RemoveStockAsync(product, removedQuantity);
            // Assert
            await context.Entry(product).ReloadAsync(); // Reload the product from the database to get the updated quantity
            Assert.AreEqual(expectedQuantity, product.Quantity);
        }
        [TestMethod]
        public async Task AddProduct_AddsProductToDB()
        {
            // Arrange
         
            var product = new Product("Test Product2", _category, 10.0m, 5, 2);

            // Act
            await _inventoryService.AddProductAsync(product);

            // Assert
            await using var context = await _factory.CreateDbContextAsync();
            Assert.IsTrue(await context.Products.AnyAsync(p => p.ProductId == product.ProductId));
        }
        [TestMethod]
        public async Task AddProduct_DoesNotAllowDuplicateProductNames()
        {
            // Arrange
            var product2 = new Product("Test Product", _category, 15.0m, 3, 1);
                      
            // Assert
          await Assert.ThrowsAsync<InvalidOperationException>( async () => await _inventoryService.AddProductAsync(product2));
        }

        [TestMethod]
        public async Task RemoveProduct_RemovesProductFromDB()
        {
            // Arrange
            var productToRemove = new Product("Product To Remove", _category, 20.0m, 5, 2);
            // Act
            await _inventoryService.AddProductAsync(productToRemove);
            await _inventoryService.RemoveProductAsync(productToRemove);
            // Assert
            await using var context = await _factory.CreateDbContextAsync();
            Assert.IsFalse(await context.Products.AnyAsync(p => p.ProductId == productToRemove.ProductId));
        }

        [TestMethod]
        public async Task RemoveProduct_DoesNotRemoveProductIfNotInList()
        {
            // Arrange
            var productToRemove = new Product("Product To Remove", _category, 20.0m, 5, 2);


            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _inventoryService.RemoveProductAsync(productToRemove));

        }
        [TestMethod]
        public async Task AddStock_WithNegativeQuantity_DoesNotAddStock()
        {
            
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _inventoryService.AddStockAsync(_product, -5));

        }
        [TestMethod]
        public async Task RemoveStock_WithNegativeQuantity_ThrowsArgumentException()
        {

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _inventoryService.RemoveStockAsync(_product, -5));
        }
        [TestMethod]
        public async Task RemoveStock_InsufficientStock_ThrowsArgumentException()
        {

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _inventoryService.RemoveStockAsync(_product, 15));
        }
        [TestMethod]
        public async Task UpdateProduct_ShouldThrow_WhenNameAlreadyExists()
        {
            //Arrange
            var existingProduct = new Product("Existing Product", _category, 20.0m, 5, 2);
            await _inventoryService.AddProductAsync(existingProduct);
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _inventoryService.UpdateProductAsync(_product, "Existing Product", _category, 15.0m, 3, 1, existingProduct.ImagePath));
        }
        [TestMethod]
        public async Task UpdateProduct_ShouldAllowProductToKeepItsExistingName()
        {
            //Arrange
            var existingProduct = new Product("Existing Product2", _category, 20.0m, 5, 2);
            
            await _inventoryService.AddProductAsync(existingProduct);
            // Act
            await _inventoryService.UpdateProductAsync(existingProduct, "Existing Product2", _category, 25.0m, 10, 3,existingProduct.ImagePath);
            //Assert
            await using var context = await _factory.CreateDbContextAsync();

            var updatedProduct = await context.Products
                .FirstAsync(p => p.ProductId == existingProduct.ProductId);

            Assert.AreEqual("Existing Product2", updatedProduct.ProductName);
            Assert.AreEqual(25.0m, updatedProduct.Price);
            Assert.AreEqual(10, updatedProduct.Quantity);
            Assert.AreEqual(3, updatedProduct.LowStockThreshold);
        }
    }
}
