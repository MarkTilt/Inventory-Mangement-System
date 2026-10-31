using InventorySystem.Core.Data;
using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;


namespace InventorySystem.Core.Services
{
    public class ActivityService
    {
        private readonly InventoryDbContext _context;
        public ActivityService(InventoryDbContext context)
        {
            _context = context;
        }

        public void SetUpdateDescription(Activities activity, Product product)
        {
            List<string> changes = new List<string>();

            if (activity.OldProductName != product.ProductName)
            {
                changes.Add($"Product Name changed from '{activity.OldProductName}' to '{product.ProductName}'");
            }
            if (activity.OldProductCategory != product.Category.CategoryName)
            {
                changes.Add($"Product '{product.ProductName}' Category changed from '{activity.OldProductCategory}' to '{product.Category.CategoryName}'");
            }
            if (activity.OldProductPrice != product.Price)
            {
                changes.Add($"Product '{product.ProductName}' Price changed from 'R{activity.OldProductPrice}' to 'R{product.Price}'");
            }
            if (activity.OldProductQuantity != product.Quantity)
            {
                changes.Add($"Product '{product.ProductName}' Quantity changed from '{activity.OldProductQuantity}' to '{product.Quantity}'");
            }
            if (activity.OldProductLowStockThreshold != product.LowStockThreshold)
            {
                changes.Add($"Product '{product.ProductName}' Low Stock Threshold changed from '{activity.OldProductLowStockThreshold}' to '{product.LowStockThreshold}'");
            }

            activity.Description = string.Join("\n", changes);
            if (!string.IsNullOrEmpty(activity.Description))
            {
                activity.Time = DateTime.Now;
                _context.Activities.Add(activity);
                
            }
        }
        public void SetAddDescription(Activities activity, Product product)
        {
            activity.Description = $"Product '{product.ProductName}' added with Category '{product.Category.CategoryName}'\nPrice '{product.Price}'\nQuantity '{product.Quantity}'\nLow Stock Threshold '{product.LowStockThreshold}'.";
            activity.Time = DateTime.Now;
            _context.Activities.Add(activity);
           
        }
        public void RemoveProductDescription(Activities activity, Product product)
        {
            activity.Description = $"Product '{product.ProductName}' removed from inventory.";
            activity.Time = DateTime.Now;
            _context.Activities.Add(activity);
          

        }
        
        public void AddCategoryDescription(Activities activity, Category category)
        {
            activity.Description = $"Category '{category.CategoryName}' added";
            activity.Time = DateTime.Now;
            _context.Activities.Add(activity);
           
        }
        public void RemoveCategoryDescription(Activities activity, Category category)
        {
            activity.Description = $"Category '{category.CategoryName}' removed from inventory.";
            activity.Time = DateTime.Now;
            _context.Activities.Add(activity);
           
        }
        public void UpdateCategoryDescription(Activities activity, Category category)
        {
            if (activity.OldCategoryName != category.CategoryName)
            {
                activity.Description = $"Category Name changed from '{activity.OldCategoryName}' to '{category.CategoryName}'";
            }
            if (!string.IsNullOrEmpty(activity.Description))
            {
                activity.Time = DateTime.Now;
                _context.Activities.Add(activity);
              
            }
        }
    }
}
