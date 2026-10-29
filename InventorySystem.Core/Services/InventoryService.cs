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
            ArgumentNullException.ThrowIfNull(product);
            if (!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException(
                    "The product does not exist in the inventory.");
            }
            if (quantityToAdd <= 0)
            {
                throw new ArgumentException("Quantity to add cannot be zero or negative.");
            }
            Activities productActivity = new Activities(product, "Stock added");

            product.Quantity = checked(product.Quantity + quantityToAdd);
            productActivity.SetUpdateDescription(product);
        }

        public void RemoveStock(Product product, int quantityToRemove)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException(
                    "The product does not exist in the inventory.");
            }
            if (quantityToRemove <= 0 )
            {
                throw new ArgumentException("Quantity to remove cannot be zero or negative.");
            }
            if (product.Quantity < quantityToRemove)
            {
                throw new ArgumentException("Insufficient stock to remove.");
            }
            Activities productActivity = new Activities(product, "Stock removed");

            product.Quantity -= quantityToRemove;
            productActivity.SetUpdateDescription(product);

        }

        public void AddProduct(Product product)
        {
            
            
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
            product.ProductName = product.ProductName.Trim();

            if (ProductList.Products.Any(p => string.Equals(p.ProductName.Trim(), product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A product with the name '{product.ProductName}' already exists.");
            }
            Activities productActivity = new Activities("Product added");

            
            ProductList.Products.Add(product);
            productActivity.SetAddDescription(product);
        }
        public void RemoveProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            
            if (!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            Activities productActivity = new Activities("Product removed");
            ProductList.Products.Remove(product);
            productActivity.RemoveProductDescription(product);
        }


        public void UpdateProduct(Product product, string newName, Category newCategory, decimal newPrice, int newQuantity, int newLowStockThreshold)
        {
            ArgumentNullException.ThrowIfNull(product);
            
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (newCategory == null)
            {
                throw new ArgumentException("Product category cannot be empty.");
            }
            if (!ProductList.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            newName = newName.Trim();
            if (ProductList.Products.Any(p => p.ProductId != product.ProductId && string.Equals(p.ProductName.Trim(), newName, StringComparison.OrdinalIgnoreCase)))
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
            Activities productActivity = new Activities(product, "Product updated");

            product.ProductName = newName.Trim();
            product.Category = newCategory;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            product.LowStockThreshold = newLowStockThreshold;
            productActivity.SetUpdateDescription(product);
        }

        public void AddCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            category.CategoryName = category.CategoryName.Trim();

            if (CategoryList.Categories.Any(c => string.Equals(c.CategoryName.Trim(), category.CategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.CategoryName}' already exists.");
            }
            Activities categoryActivity = new Activities(category, "Category added");
            
            CategoryList.Categories.Add(category);
            categoryActivity.AddCategoryDescription(category);
        }
        public void RemoveCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            
            if (!CategoryList.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            if(ProductList.Products.Any(p => p.Category.CategoryId == category.CategoryId))
            {
                throw new InvalidOperationException($"Cannot remove category '{category.CategoryName}' because it is associated with existing products.");
            }
            Activities categoryActivity = new Activities(category, "Category removed");
            CategoryList.Categories.Remove(category);
            categoryActivity.RemoveCategoryDescription(category);
        }


        public void UpdateCategory(Category category, string newName)
        {
            ArgumentNullException.ThrowIfNull(category);
          
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            if (!CategoryList.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            newName = newName.Trim();
            if (CategoryList.Categories.Any(c => c.CategoryId != category.CategoryId && string.Equals(c.CategoryName.Trim(), newName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{newName}' already exists.");
            }
            Activities categoryActivity = new Activities(category, "Category Updated");


            category.CategoryName = newName.Trim();
            categoryActivity.UpdateCategoryDescription(category);
        }
    
          
        
    }
}
