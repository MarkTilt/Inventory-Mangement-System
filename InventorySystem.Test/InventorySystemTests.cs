using InventorySystem.Core.Models;
namespace InventorySystem.Test
{
    [TestClass]
    public sealed class InventorySystemTests
    {
        //Testing InventoryServices

        [TestMethod]
        public void AddStockToProduct_IncreasesQuantity()
        {
            // Arrange
            var category = new Category(1, "Test Category");
            var Product = new Product(1, "Test Product", category, 10.0m, 5, 2);
            var inventoryService = new InventoryService();
            // Act
            inventoryService.AddStock(Product, 3);
            // Assert
            Assert.AreEqual(8, Product.Quantity);
        }

        public class InventoryService
        {
            public void AddStock(Product product, int quantityToAdd)
            {
                product.Quantity += quantityToAdd;
            }
        }
    }
}
