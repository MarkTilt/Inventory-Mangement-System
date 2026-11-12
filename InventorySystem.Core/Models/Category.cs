using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventorySystem.Core.Models
{
    public class Category
    {
        public Guid CategoryId { get; set;  }
        [MaxLength(50)]
        public string CategoryName { get; set; }

        public Category( string categoryName)
        {
            CategoryId = Guid.NewGuid();
            CategoryName = categoryName;
        }
    }
}
