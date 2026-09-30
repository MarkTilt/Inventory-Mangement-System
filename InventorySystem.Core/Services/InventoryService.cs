using InventorySystem.Core.Models;
using InventorySystem.Core.List;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystem.Core.Services
{
    public class InventoryService
    {
        public void AddStock(Product product, int quantityToAdd)
        {
            product.Quantity += quantityToAdd;
        }

        public void RemoveStock(Product product, int quantityToRemove)
        {
            product.Quantity -= quantityToRemove;
        }

        public void AddProduct(Product product)
        {
            if (ProductList.Products.Any(p => p.ProductName == product.ProductName))
            {
                return;
            }
                ProductList.Products.Add(product);
            
        }
    }
}
