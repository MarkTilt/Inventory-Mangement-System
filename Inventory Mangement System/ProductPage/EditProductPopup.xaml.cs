using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.List;


namespace Inventory_Mangement_System;

public partial class EditProductPopup : Popup
{
    InventoryService inventoryService = new InventoryService();
    private readonly ProductPage _productPage;

    // The original product
    public Product Product { get; set; }

    // Temporary copy for editing
    public string EditedName { get; set; }
    public Category EditedCategory { get; set; }
    public decimal EditedPrice { get; set; }
    public int EditedQuantity { get; set; }
    public int EditedLowStockThreshold { get; set; }

    public List<Category> Categories { get; set; }

    public EditProductPopup(Product product, ProductPage page)
	{
		InitializeComponent();
		
        
        Product = product;
        _productPage = page;
        Categories = CategoryList.Categories;

        // Copy the original values
        EditedName = product.ProductName;
        EditedPrice = product.Price;
        EditedQuantity = product.Quantity;
        EditedLowStockThreshold = product.LowStockThreshold;
        EditedCategory = product.Category;

        BindingContext = this;
    }

    private async void SaveOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            inventoryService.UpdateProduct(
                Product,
                EditedName,
                EditedCategory,
                EditedPrice,
                EditedQuantity,
                EditedLowStockThreshold);

            await CloseAsync();
            _productPage.LoadProducts();
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!
                .DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void CancelOn_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}