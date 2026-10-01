using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.List;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            ProductList.Products.Clear();
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
        public void AddProduct_AddsProductToList()
        {
            // Arrange
         
            var product = new Product("Test Product", category, 10.0m, 5, 2);

            // Act
            inventoryService.AddProduct(product);

            // Assert
            Assert.Contains(product, ProductList.Products);


        }
        [TestMethod]
        public void AddProduct_DoNotAllowDuplicateProductNames()
        {
            // Arrange
            var product1 = new Product("Test Product", category, 10.0m, 5, 2);
            var product2 = new Product("Test Product", category, 15.0m, 3, 1);
            // Act
            inventoryService.AddProduct(product1);
            
            // Assert
           Assert.Throws<InvalidOperationException>(() => inventoryService.AddProduct(product2));
        }

        [TestMethod]
        public void RemoveProduct_RemovesProductFromList()
        {
            // Arrange
            var ProductToRemove = new Product("Product To Remove", category, 20.0m, 5, 2);
            // Act
            inventoryService.AddProduct(ProductToRemove);
            inventoryService.RemoveProduct(ProductToRemove);
            // Assert
            Assert.IsFalse(ProductList.Products.Contains(ProductToRemove));
        }

        [TestMethod]
        public void RemoveProduct_DontRemoveProductIfNotInList()
        {
            // Arrange
            var ProductToRemove = new Product("Product To Remove", category, 20.0m, 5, 2);


            // Assert
            Assert.Throws<InvalidOperationException>(() => inventoryService.RemoveProduct(ProductToRemove));

        }
        [TestMethod]
        public void AddStock_WithNegativeQuantity_DoesNotAddStock()
        {
            
            //Assert
            Assert.Throws<ArgumentException>(() => inventoryService.AddStock(product, -5));

        }
        [TestMethod]
        public void RemoveStock_WithNegativeQuantity_ThrowsArgumentException()
        {

            //Assert
            Assert.Throws<ArgumentException>(() => inventoryService.RemoveStock(product, -5));
        }
        [TestMethod]
        public void RemoveStock_InsufficientStock_ThrowsArgumentException()
        {

            //Assert
            Assert.Throws<ArgumentException>(() => inventoryService.RemoveStock(product, 15));
        }
        [TestMethod]
        public void UpdateProduct_ShouldThrow_WhenNameAlreadyExists()
        {
            //Arrange
            var existingProduct = new Product("Existing Product", category, 20.0m, 5, 2);
            inventoryService.AddProduct(existingProduct);
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => inventoryService.UpdateProduct(product, "Existing Product", category, 15.0m, 3, 1));
        }
        [TestMethod]
        public void UpdateProduct_ShouldAllowProductToKeepItsExistingName()
        {
            //Arrange
            var existingProduct = new Product("Existing Product2", category, 20.0m, 5, 2);
            inventoryService.AddProduct(existingProduct);
            // Act
            inventoryService.UpdateProduct(existingProduct, "Existing Product2", category, 25.0m, 10, 3);
            //Assert
            Assert.AreEqual("Existing Product2", existingProduct.ProductName);

        }
    }
}
