using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.List;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem.Core.Data;
using Microsoft.EntityFrameworkCore;
namespace InventorySystem.Test
{
    [TestClass]
    public sealed class InventorySystemTests
    {
        //Testing InventoryServices

        private Category _category;
        private InventoryService _inventoryService;
        private ActivityService _activityService;
        private InventoryDbContext _context;
        private Product _product;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventoryDbContext(options);

            _activityService = new ActivityService(_context);
            _inventoryService = new InventoryService(_context, _activityService);

            _category = new Category("Test Category");
            _context.Categories.Add(_category);
           await _context.SaveChangesAsync();
          
            _product = new Product("Test Product", _category, 10.0m, 0, 2);
           await _inventoryService.AddProductAsync(_product);
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
        }

        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(10, 5, 15)]
        public async Task AddStockToProduct_IncreasesQuantity(int productQuantity, int addedQuantity, int expectedQuantity)
        {
            // Arrange
            _product.Quantity = productQuantity;
            await _context.SaveChangesAsync();

            // Act
            await _inventoryService.AddStockAsync(_product, addedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, _product.Quantity);
        }
        [TestMethod]
        [DataRow(8, 3, 5)]
        [DataRow(15, 5, 10)]
        public async Task RemoveStockFromProduct_DecreasesQuantity(int productQuantity, int removedQuantity, int expectedQuantity)
        {
            // Arrange

            _product.Quantity = productQuantity;
            await _context.SaveChangesAsync();
            // Act
            await _inventoryService.RemoveStockAsync(_product, removedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, _product.Quantity);
        }
        [TestMethod]
        public async Task AddProduct_AddsProductToDB()
        {
            // Arrange
         
            var product = new Product("Test Product2", _category, 10.0m, 5, 2);

            // Act
            await _inventoryService.AddProductAsync(product);

            // Assert
            Assert.Contains(product, _context.Products.ToList());


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
            await _context.SaveChangesAsync();
            await _inventoryService.RemoveProductAsync(productToRemove);
            await _context.SaveChangesAsync();
            // Assert
            Assert.IsFalse(await _context.Products.AnyAsync(p => p.ProductId == productToRemove.ProductId));
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
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _inventoryService.UpdateProductAsync(_product, "Existing Product", _category, 15.0m, 3, 1));
        }
        [TestMethod]
        public async Task UpdateProduct_ShouldAllowProductToKeepItsExistingName()
        {
            //Arrange
            var existingProduct = new Product("Existing Product2", _category, 20.0m, 5, 2);
            await _inventoryService.AddProductAsync(existingProduct);
            await _context.SaveChangesAsync();
            // Act
            await _inventoryService.UpdateProductAsync(existingProduct, "Existing Product2", _category, 25.0m, 10, 3);
            await _context.SaveChangesAsync();
            //Assert
            Assert.AreEqual("Existing Product2", existingProduct.ProductName);

        }
    }
}
