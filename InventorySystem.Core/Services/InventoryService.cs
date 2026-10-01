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
    }
}
