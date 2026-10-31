using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using InventorySystem.Core.Data;
using System.Net.Http.Headers;

namespace InventorySystem.Core.Services
{
    public class InventoryService
    {
        private readonly InventoryDbContext _context;
        private readonly ActivityService _activityService;
        public InventoryService(InventoryDbContext context, ActivityService activityService)
        {
            _context = context;
            _activityService = activityService;
        }
        public  int LowStockCount { 
            get 
            {
                return _context.Products.Count(p => p.Quantity <= p.LowStockThreshold);
            } 
        }
        public async Task AddStockAsync(Product product, int quantityToAdd)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (!_context.Products.Contains(product))
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
            _activityService.SetUpdateDescription(productActivity, product);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStockAsync(Product product, int quantityToRemove)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (!_context.Products.Contains(product))
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
            _activityService.SetUpdateDescription(productActivity, product);
            await _context.SaveChangesAsync();

        }

        public async Task AddProductAsync(Product product)
        {

            ArgumentNullException.ThrowIfNull(product);
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

            if (_context.Products.Any(p => string.Equals(p.ProductName.Trim(), product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A product with the name '{product.ProductName}' already exists.");
            }
            Activities productActivity = new Activities("Product added");


            _context.Products.Add(product);
            _activityService.SetAddDescription(productActivity, product);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveProductAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            
            if (!_context.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            Activities productActivity = new Activities("Product removed");
            _context.Products.Remove(product);
            _activityService.RemoveProductDescription(productActivity, product);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product, string newName, Category newCategory, decimal newPrice, int newQuantity, int newLowStockThreshold)
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
            if (!_context.Products.Contains(product))
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            newName = newName.Trim();
            if (_context.Products.Any(p => p.ProductId != product.ProductId && string.Equals(p.ProductName.Trim(), newName, StringComparison.OrdinalIgnoreCase)))
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
            Activities productActivity = new Activities(product, "Product updated");

            product.ProductName = newName.Trim();
            product.Category = newCategory;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            product.LowStockThreshold = newLowStockThreshold;
            _activityService.SetUpdateDescription(productActivity, product);
            await _context.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            category.CategoryName = category.CategoryName.Trim();

            if (_context.Categories.Any(c => string.Equals(c.CategoryName.Trim(), category.CategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.CategoryName}' already exists.");
            }
            Activities categoryActivity = new Activities(category, "Category added");
            
            _context.Categories.Add(category);
            _activityService.AddCategoryDescription(categoryActivity, category);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCategoryAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            
            if (!_context.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            if(_context.Products.Any(p => p.Category.CategoryId == category.CategoryId))
            {
                throw new InvalidOperationException($"Cannot remove category '{category.CategoryName}' because it is associated with existing products.");
            }
            Activities categoryActivity = new Activities(category, "Category removed");
            _context.Categories.Remove(category);
            _activityService.RemoveCategoryDescription(categoryActivity, category);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateCategoryAsync(Category category, string newName)
        {
            ArgumentNullException.ThrowIfNull(category);
          
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            if (!_context.Categories.Contains(category))
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            newName = newName.Trim();
            if (_context.Categories.Any(c => c.CategoryId != category.CategoryId && string.Equals(c.CategoryName.Trim(), newName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{newName}' already exists.");
            }
            Activities categoryActivity = new Activities(category, "Category Updated");


            category.CategoryName = newName.Trim();
            _activityService.UpdateCategoryDescription(categoryActivity, category);
            await _context.SaveChangesAsync();
        }
    
          
        
    }
}
