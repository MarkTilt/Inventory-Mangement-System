namespace InventorySystem.Core.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }

        public Product( string productName, Category category, decimal price, int quantity, int lowStockThreshold)
        {
            ProductId = Guid.NewGuid();
            ProductName = productName;
            Category = category;
            Price = price;
            Quantity = quantity;
            LowStockThreshold = lowStockThreshold;
        }

    }
}
