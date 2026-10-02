using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystem.Core.List
{
    public class CategoryList
    {
        public static List<Category> Categories { get; } = new()
        {
            new Category("Electronics"),
            new Category("Food")
        };
    }
}
