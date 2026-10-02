using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystem.Core.List
{
    public class ProductList
    {
        public static List<Product> Products { get; } = new()
        {
            new Product("Mouse",   CategoryList.Categories[0] , 50m, 10,3),
            new Product("Keyboard",CategoryList.Categories[0] , 80m, 21,3),
            new Product("Headset", CategoryList.Categories[0], 200m, 2,3)
        };
        
    }
}
