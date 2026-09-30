namespace InventorySystem.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }

        public Product(int productId, string productName, string category, decimal price, int quantity, int lowStockThreshold)
        {
            ProductId = productId;
            ProductName = productName;
            Category = category;
            Price = price;
            Quantity = quantity;
            LowStockThreshold = lowStockThreshold;
        }

    }
}
