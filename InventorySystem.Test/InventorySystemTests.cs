using InventorySystem.Core.Models;
namespace InventorySystem.Test
{
    [TestClass]
    public sealed class InventorySystemTests
    {
        //Testing InventoryServices

        private Category category;
        private InventoryService inventoryService;

        [TestInitialize]
        public void Setup()
        {
            category = new Category( "Test Category");
            inventoryService = new InventoryService();
        }

        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(10, 5, 15)]
        public void AddStockToProduct_IncreasesQuantity(int productQuantity, int addedQuantity, int expectedQuantity)
        {
            // Arrange
            
            var Product = new Product( "Test Product", category, 10.0m, productQuantity, 2);
          
            // Act
            inventoryService.AddStock(Product, addedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, Product.Quantity);
        }
        [TestMethod]
        [DataRow(8, 3, 5)]
        [DataRow(15, 5, 10)]
        public void RemoveStockFromProduct_DecreasesQuantity(int productQuantity, int removedQuantity, int expectedQuantity)
        {
            // Arrange
           
            var Product = new Product( "Test Product", category, 10.0m, productQuantity, 2);
            
            // Act
            inventoryService.RemoveStock(Product, removedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, Product.Quantity);
        }

        public class InventoryService
        {
            public void AddStock(Product product, int quantityToAdd)
            {
                product.Quantity += quantityToAdd;
            }

            public void RemoveStock(Product product, int quantityToRemove)
            {
                product.Quantity -= quantityToRemove;
            }
        }
    }
}
