using InventorySystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystem.Core.List
{
    public static class ActivityList
    {
        public static List<ProductActivity> Activities { get; }
            = new List<ProductActivity>();
    }
}
