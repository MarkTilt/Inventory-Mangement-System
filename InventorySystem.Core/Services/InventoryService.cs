using InventorySystem.Core.Models;
using InventorySystem.Core.List;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystem.Core.Services
{
    public class InventoryService
    {
        public  int LowStockCount { 
            get 
            {
                return ProductList.Products.Count(p => p.Quantity <= p.LowStockThreshold);
            } 
        }
        public void AddStock(Product product, int quantityToAdd)
        {
            if(quantityToAdd < 0)
            {
                throw new ArgumentException("Quantity to add cannot be negative.");
            }
                product.Quantity += quantityToAdd;
        }

        public void RemoveStock(Product product, int quantityToRemove)
        {
            if(quantityToRemove < 0 )
            {
                throw new ArgumentException("Quantity to remove cannot be negative.");
            }
            if (product.Quantity < quantityToRemove)
            {
                throw new ArgumentException("Insufficient stock to remove.");
            }
            product.Quantity -= quantityToRemove;
           
        }

        public void AddProduct(Product product)
        {

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
           
            if(product.Category == null)
            {
                throw new ArgumentException("Product must have a category.");
            }
            if (product.Quantity < 0 )
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }
            
            if (product.LowStockThreshold < 0 )
            {
                throw new ArgumentException("Low stock threshold cannot be negative.");
            }
           

            if (ProductList.Products.Any(p => p.ProductName.Trim().ToLower() == product.ProductName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A product with the name '{product.ProductName}' already exists.");
            }
                ProductList.Products.Add(product);
        }
        public void RemoveProduct(Product product)
        {
            if(!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            ProductList.Products.Remove(product);
        }


        public void UpdateProduct(Product product, string newName, Category newCategory, decimal newPrice, int newQuantity, int newLowStockThreshold)
        {

            if (ProductList.Products.Any(p => p.ProductId != product.ProductId && p.ProductName.Trim().ToLower() == newName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A product with the name '{newName}' already exists.");
            }
            if (newPrice < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            if (newQuantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }
            if (newLowStockThreshold < 0)
            {
                throw new ArgumentException("Low stock threshold cannot be negative.");
            }

            product.ProductName = newName;
            product.Category = newCategory;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            product.LowStockThreshold = newLowStockThreshold;
        }
        public void LowOnStock(Product product)
        {
            if (product.Quantity <= product.LowStockThreshold)
            {
                // Trigger an alert (for example, log a message or send a notification)
            }      
        }

        public void AddCategory(Category category)
        {
            if (CategoryList.Categories.Any(c => c.CategoryName.Trim().ToLower() == category.CategoryName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A category with the name '{category.CategoryName}' already exists.");
            }
            CategoryList.Categories.Add(category);
        }
        public void RemoveCategory(Category category)
        {
            if (!CategoryList.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            CategoryList.Categories.Remove(category);
        }


        public void UpdateCategory(Category category, string newName)
        {

            if (CategoryList.Categories.Any(c => c.CategoryId != category.CategoryId && c.CategoryName.Trim().ToLower() == newName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A category with the name '{newName}' already exists.");
            }
            

            category.CategoryName = newName;
        }
    
          
        
    }
}
