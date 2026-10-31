using InventorySystem.Core.List;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
namespace InventorySystem.Core.Models
{
    public class Activities
    {
        public Guid ActivityId { get; set; } = Guid.NewGuid();

        // Properties to store the old product details
        public string OldProductName { get; set; }
        public string OldProductCategory { get; set; }
        public decimal OldProductPrice { get; set; }
        public int OldProductQuantity { get; set; }
        public int OldProductLowStockThreshold { get; set; }

        //Properties to store old Category details
        public string OldCategoryName { get; set; }

        public string Description { get; set; }
        public string ChangeReason { get; set; }
        public DateTime Time { get; set; }

        

        //When adding new products
        public Activities(string changeReason)
        {
            ChangeReason = changeReason;
        }
       
        //When updating existing products
        public Activities(Product product, string changeReason)
        {
            OldProductName = product.ProductName;
            OldProductCategory = product.Category.CategoryName;
            OldProductPrice = product.Price;
            OldProductQuantity = product.Quantity;
            OldProductLowStockThreshold = product.LowStockThreshold;

            
            ChangeReason = changeReason;
            
        }
        public Activities(Category category, string changeReason)
        {
            OldCategoryName = category.CategoryName;
            ChangeReason = changeReason;
        }

      

    }
}
