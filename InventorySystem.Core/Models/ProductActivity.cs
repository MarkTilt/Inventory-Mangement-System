using System;
using System.Collections.Generic;
using System.Text;
using InventorySystem.Core.List;
namespace InventorySystem.Core.Models
{
    public class ProductActivity
    {
        // Properties to store the old product details
        public string OldProductName { get; set; }
        public string OldProductCategory { get; set; }
        public decimal OldProductPrice { get; set; }
        public int OldProductQuantity { get; set; }
        public int OldProductLowStockThreshold { get; set; }

        public string Description { get; set; }
        public string ChangeReason { get; set; }
        public DateTime Time { get; set; }

        

        //When adding new products
        public ProductActivity(string changeReason)
        {
            ChangeReason = changeReason;
        }
       
        //When updating existing products
        public ProductActivity(Product product, string changeReason)
        {
            OldProductName = product.ProductName;
            OldProductCategory = product.Category.CategoryName;
            OldProductPrice = product.Price;
            OldProductQuantity = product.Quantity;
            OldProductLowStockThreshold = product.LowStockThreshold;

            
            ChangeReason = changeReason;
            
        }
        public void SetUpdateDescription(Product product)
        {
            List<string> changes = new List<string>();

            if(OldProductName != product.ProductName)
            {
                changes.Add($"Product Name changed from '{OldProductName}' to '{product.ProductName}'");
            }
            if(OldProductCategory != product.Category.CategoryName)
            {
                changes.Add($"Product '{product.ProductName}' Category changed from '{OldProductCategory}' to '{product.Category.CategoryName}'");
            }
            if(OldProductPrice != product.Price)
            {
                changes.Add($"Product '{product.ProductName}' Price changed from '{OldProductPrice}' to '{product.Price}'");
            }
            if(OldProductQuantity != product.Quantity)
            {
                changes.Add($"Product '{product.ProductName}' Quantity changed from '{OldProductQuantity}' to '{product.Quantity}'");
            }
            if(OldProductLowStockThreshold != product.LowStockThreshold)
            {
                changes.Add($"Product '{product.ProductName}' Low Stock Threshold changed from '{OldProductLowStockThreshold}' to '{product.LowStockThreshold}'");
            }

            Description = string.Join("\n", changes);
            if(!string.IsNullOrEmpty(Description))
            {
                Time = DateTime.Now;
                ActivityList.Activities.Add(this);
            }
        }
        public void SetAddDescription(Product product)
        {
            Description = $"Product '{product.ProductName}' added with Category '{product.Category.CategoryName}'\n Price '{product.Price}'\n Quantity '{product.Quantity}'\n Low Stock Threshold '{product.LowStockThreshold}'.";
            Time = DateTime.Now;
            ActivityList.Activities.Add(this);
        }
        public void RemoveProductDescription(Product product)
        {
            Description = $"Product '{product.ProductName}' removed from inventory.";
            Time = DateTime.Now;
            ActivityList.Activities.Add(this);

        }

    }
}
