using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.List;
namespace InventorySystem.Test
{
    [TestClass]
    public sealed class InventorySystemTests
    {
        //Testing InventoryServices

        private Category category;
        private InventoryService inventoryService;
        private Product product;

        [TestInitialize]
        public void Setup()
        {
            category = new Category("Test Category");
            inventoryService = new InventoryService();
            product = new Product("Test Product", category, 10.0m, 0, 2);
        }

        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(10, 5, 15)]
        public void AddStockToProduct_IncreasesQuantity(int productQuantity, int addedQuantity, int expectedQuantity)
        {
            // Arrange
            product.Quantity = productQuantity;

            // Act
            inventoryService.AddStock(product, addedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, product.Quantity);
        }
        [TestMethod]
        [DataRow(8, 3, 5)]
        [DataRow(15, 5, 10)]
        public void RemoveStockFromProduct_DecreasesQuantity(int productQuantity, int removedQuantity, int expectedQuantity)
        {
            // Arrange

            product.Quantity = productQuantity;

            // Act
            inventoryService.RemoveStock(product, removedQuantity);
            // Assert
            Assert.AreEqual(expectedQuantity, product.Quantity);
        }
        [TestMethod]
        public void AddAProductToProductList()
        {
            // Arrange
         
            var product = new Product("Test Product", category, 10.0m, 5, 2);

            // Act
            inventoryService.AddProduct(product);

            // Assert
            Assert.Contains(product, ProductList.Products);


        }
        public void AddProduct_DoNotAllowDuplicateProductNames()
        {
            // Arrange
            var product1 = new Product("Test Product", category, 10.0m, 5, 2);
            var product2 = new Product("Test Product", category, 15.0m, 3, 1);
            // Act
            inventoryService.AddProduct(product1);
            inventoryService.AddProduct(product2);
            // Assert
            var productsWithSameName = ProductList.Products.Where(p => p.ProductName.ToLower().Trim() == "Test Product".ToLower().Trim()).ToList();
            Assert.AreEqual(1, productsWithSameName.Count);
        }

    }
}
