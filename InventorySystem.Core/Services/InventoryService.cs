using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using InventorySystem.Core.Data;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

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
        public async Task<int> GetLowStockCountAsync() { 
            return await _context.Products.CountAsync(p => p.Quantity <= p.LowStockThreshold);
        }
        public async Task AddStockAsync(Product product, int quantityToAdd)
        {
            ArgumentNullException.ThrowIfNull(product);
            var existingProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                throw new InvalidOperationException(
                    "The product does not exist in the inventory.");
            }
            if (quantityToAdd <= 0)
            {
                throw new ArgumentException("Quantity to add cannot be zero or negative.");
            }
            Activities productActivity = new Activities(existingProduct, "Stock added");

            existingProduct.Quantity = checked(existingProduct.Quantity + quantityToAdd);
            _activityService.SetUpdateDescription(productActivity, existingProduct);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStockAsync(Product product, int quantityToRemove)
        {
            ArgumentNullException.ThrowIfNull(product);
            var existingProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                throw new InvalidOperationException(
                    "The product does not exist in the inventory.");
            }
            if (quantityToRemove <= 0 )
            {
                throw new ArgumentException("Quantity to remove cannot be zero or negative.");
            }
            if (existingProduct.Quantity < quantityToRemove)
            {
                throw new ArgumentException("Insufficient stock to remove.");
            }
            Activities productActivity = new Activities(existingProduct, "Stock removed");

            existingProduct.Quantity -= quantityToRemove;
            _activityService.SetUpdateDescription(productActivity, existingProduct);
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
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == product.Category.CategoryId);
            if (existingCategory == null)
            {
                throw new InvalidOperationException("The selected category does not exist.");
            }
            product.Category = existingCategory;
            if (product.Quantity < 0 )
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }
            
            if (product.LowStockThreshold < 0 )
            {
                throw new ArgumentException("Low stock threshold cannot be negative.");
            }
            product.ProductName = product.ProductName.Trim();

            if (await _context.Products.AnyAsync(p => p.ProductName == product.ProductName))
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
            var existingProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            Activities productActivity = new Activities("Product removed");
            _context.Products.Remove(existingProduct);
            _activityService.RemoveProductDescription(productActivity, existingProduct);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product, string newName, Category newCategory, decimal newPrice, int newQuantity, int newLowStockThreshold)
        {
            ArgumentNullException.ThrowIfNull(product);
            var existingProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"The product '{product.ProductName}' does not exist in the inventory.");
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (newCategory == null)
            {
                throw new ArgumentException("Product category cannot be empty.");
            }
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == newCategory.CategoryId);
            if (existingCategory == null)
            {
                throw new InvalidOperationException($"The category '{newCategory.CategoryName}' does not exist in the inventory.");
            }
            newName = newName.Trim();
            if (await _context.Products.AnyAsync(p => p.ProductId != existingProduct.ProductId && p.ProductName == newName))
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
            Activities productActivity = new Activities(existingProduct, "Product updated");

            existingProduct.ProductName = newName;
            existingProduct.Category = existingCategory;
            existingProduct.Price = newPrice;
            existingProduct.Quantity = newQuantity;
            existingProduct.LowStockThreshold = newLowStockThreshold;
            _activityService.SetUpdateDescription(productActivity, existingProduct);
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

            if (await _context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName))
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
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if (existingCategory == null)
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            if(await _context.Products.AnyAsync(p => p.Category.CategoryId == category.CategoryId))
            {
                throw new InvalidOperationException($"Cannot remove category '{category.CategoryName}' because it is associated with existing products.");
            }
            Activities categoryActivity = new Activities(existingCategory, "Category removed");
            _context.Categories.Remove(existingCategory);
            _activityService.RemoveCategoryDescription(categoryActivity, existingCategory);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateCategoryAsync(Category category, string newName)
        {
            ArgumentNullException.ThrowIfNull(category);
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if(existingCategory == null)
            {
                throw new InvalidOperationException($"The category '{category.CategoryName}' does not exist in the inventory.");
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            newName = newName.Trim();
            if (await _context.Categories.AnyAsync(c => c.CategoryId != category.CategoryId && c.CategoryName == newName))
            {
                throw new InvalidOperationException($"A category with the name '{newName}' already exists.");
            }
            Activities categoryActivity = new Activities(existingCategory, "Category Updated");


            existingCategory.CategoryName = newName;
            _activityService.UpdateCategoryDescription(categoryActivity, existingCategory);
            await _context.SaveChangesAsync();
        }
    
          
        
    }
}
