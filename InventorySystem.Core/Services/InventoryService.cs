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
            Activities productActivity = new Activities(product, "Stock added");
            if (quantityToAdd < 0)
            {
                throw new ArgumentException("Quantity to add cannot be negative.");
            }
                product.Quantity += quantityToAdd;
                productActivity.SetUpdateDescription(product);
        }

        public void RemoveStock(Product product, int quantityToRemove)
        {
            Activities productActivity = new Activities(product, "Stock removed");
            if(quantityToRemove < 0 )
            {
                throw new ArgumentException("Quantity to remove cannot be negative.");
            }
            if (product.Quantity < quantityToRemove)
            {
                throw new ArgumentException("Insufficient stock to remove.");
            }
            product.Quantity -= quantityToRemove;
            productActivity.SetUpdateDescription(product);

        }

        public void AddProduct(Product product)
        {
            Activities productActivity = new Activities("Product added");
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
                productActivity.SetAddDescription(product);
        }
        public void RemoveProduct(Product product)
        {
            Activities productActivity = new Activities("Product removed");
            if (!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            ProductList.Products.Remove(product);
            productActivity.RemoveProductDescription(product);
        }


        public void UpdateProduct(Product product, string newName, Category newCategory, decimal newPrice, int newQuantity, int newLowStockThreshold)
        {
            Activities productActivity = new Activities(product, "Product updated");
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
            if(newCategory == null)
            {
                throw new ArgumentException("Product must have a category.");
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            product.ProductName = newName;
            product.Category = newCategory;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            product.LowStockThreshold = newLowStockThreshold;
            productActivity.SetUpdateDescription(product);
        }

        public void AddCategory(Category category)
        {
            Activities categoryActivity = new Activities(category, "Category added");
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            if (CategoryList.Categories.Any(c => c.CategoryName.Trim().ToLower() == category.CategoryName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A category with the name '{category.CategoryName}' already exists.");
            }
            CategoryList.Categories.Add(category);
            categoryActivity.AddCategoryDescription(category);
        }
        public void RemoveCategory(Category category)
        {
            Activities categoryActivity = new Activities(category, "Category removed");
            if (!CategoryList.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            if(ProductList.Products.Any(p => p.Category.CategoryId == category.CategoryId))
            {
                throw new InvalidOperationException($"Cannot remove category '{category.CategoryName}' because it is associated with existing products.");
            }
            CategoryList.Categories.Remove(category);
            categoryActivity.RemoveCategoryDescription(category);
        }


        public void UpdateCategory(Category category, string newName)
        {
            Activities categoryActivity = new Activities(category, "Category Updated");
            if (CategoryList.Categories.Any(c => c.CategoryId != category.CategoryId && c.CategoryName.Trim().ToLower() == newName.Trim().ToLower()))
            {
                throw new InvalidOperationException($"A category with the name '{newName}' already exists.");
            }
            if(string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            category.CategoryName = newName;
            categoryActivity.UpdateCategoryDescription(category);
        }
    
          
        
    }
}
