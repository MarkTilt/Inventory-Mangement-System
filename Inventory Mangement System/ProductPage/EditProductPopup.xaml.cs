using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.Data;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;


namespace Inventory_Mangement_System;

public partial class EditProductPopup : Popup
{
    private readonly InventoryService _inventoryService;
    private readonly IDbContextFactory<InventoryDbContext> _contextFactory;

    // The original product
    public Product Product { get; set; }

    // Temporary copy for editing
    public string EditedName { get; set; }
    public Category EditedCategory { get; set; }
    public string EditedPrice { get; set; }
    public string EditedQuantity { get; set; }
    public string EditedLowStockThreshold { get; set; }

    public List<Category> Categories { get; set; }

    public EditProductPopup(Product product, InventoryService inventoryService, IDbContextFactory<InventoryDbContext> contextFactory)
	{
		InitializeComponent();
		
        
        Product = product;
        _inventoryService = inventoryService;
        _contextFactory = contextFactory;
        Opened += async (sender, args) =>
        {
            await LoadCategories();
        };
        

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
            await _inventoryService.UpdateProductAsync(
                Product,
                EditedName,
                EditedCategory,
                price,
                quantity,
                threshold);

            await CloseAsync();
           
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
    private async Task LoadCategories()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        Categories = await context.Categories.ToListAsync();

        EditedCategory = Categories.FirstOrDefault(
        c => c.CategoryId == Product.Category.CategoryId);

        OnPropertyChanged(nameof(Categories));
        OnPropertyChanged(nameof(EditedCategory));
    }
}