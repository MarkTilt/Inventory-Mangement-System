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
    public string EditedPrice { get; set; }
    public string EditedQuantity { get; set; }
    public string EditedLowStockThreshold { get; set; }

    public List<Category> Categories { get; set; }

    public EditProductPopup(Product product, ProductPage page)
	{
		InitializeComponent();
		
        
        Product = product;
        _productPage = page;
        Categories = CategoryList.Categories;

        // Copy the original values
        EditedName = product.ProductName;
        EditedPrice = product.Price.ToString();
        EditedQuantity = product.Quantity.ToString();
        EditedLowStockThreshold = product.LowStockThreshold.ToString();
        EditedCategory = product.Category;

        BindingContext = this;
    }

    private async void SaveOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!decimal.TryParse(EditedPrice, out decimal price))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Price", "Please enter a valid price.", "OK");
                return;
            }

            if (!int.TryParse(EditedQuantity, out int quantity))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Quantity", "Please enter a valid whole number.", "OK");
                return;
            }

            if (!int.TryParse(EditedLowStockThreshold, out int threshold))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Threshold", "Please enter a valid whole number.", "OK");
                return;
            }
            inventoryService.UpdateProduct(
                Product,
                EditedName,
                EditedCategory,
                price,
                quantity,
                threshold);

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