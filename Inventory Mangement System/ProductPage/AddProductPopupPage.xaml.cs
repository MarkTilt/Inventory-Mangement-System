
using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Data;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Mangement_System;

public partial class AddProductPopupPage : Popup
{

    private readonly InventoryService _inventoryService;
    private readonly IDbContextFactory<InventoryDbContext> _contextFactory;
    private string _productImagePath = "product_placeholder.png";


    public List<Category> Categories { get; set; } = new();
    public AddProductPopupPage(InventoryService inventoryService, IDbContextFactory<InventoryDbContext> contextFactory)
	{
        this._inventoryService = inventoryService;
        this._contextFactory = contextFactory;

        InitializeComponent();
        Opened += async (sender, args) =>
        {
            await LoadCategories();
        };
        BindingContext = this;
    }

    private async void AddOn_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (!decimal.TryParse(ProductPrice.Text, out decimal price))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Price",
                        "Please enter a valid price.", "OK");
                return;
            }

            if (!int.TryParse(ProductQuantity.Text, out int quantity))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Quantity",
                        "Please enter a valid whole number.", "OK");
                return;
            }

            if (!int.TryParse(ProductLowStockThreshold.Text, out int threshold))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Threshold",
                        "Please enter a valid whole number.", "OK");
                return;
            }

            if (ProductCategory.SelectedItem is not Category category)
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Category",
                        "Please select a category.", "OK");
                return;
            }

            Product product = new Product(
                ProductName.Text,
                category,
                price,
                quantity,
                threshold);

            product.ImagePath = _productImagePath;

            await _inventoryService.AddProductAsync(product);
            await CloseAsync();
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!
                .DisplayAlertAsync("Error", ex.Message, "OK");
        }
      
    }

    private async void CancelOn_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }
    private async Task LoadCategories()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        Categories = await context.Categories.ToListAsync();
        OnPropertyChanged(nameof(Categories));
    }

    private async void SelectImage_Clicked(object? sender, EventArgs e)
    {
        var result = await  FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image",
            FileTypes = FilePickerFileType.Images
        });
        if (result == null)
        {
            return;
        }
        _productImagePath = await SaveProductImageAsync(result);

        string fullPath = Path.Combine(FileSystem.AppDataDirectory, _productImagePath);
        ProductImage.Source = ImageSource.FromFile(fullPath);
    }
    private async Task<string> SaveProductImageAsync(FileResult image)
    {
        string imagesDirectory = Path.Combine(FileSystem.AppDataDirectory, "ProductImages");

        Directory.CreateDirectory(imagesDirectory);

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

        string destinationPath = Path.Combine(imagesDirectory, fileName);

        await using Stream sourceStream = await image.OpenReadAsync();
        await using FileStream destinationStream = File.Create(destinationPath);

        await sourceStream.CopyToAsync(destinationStream);

        return Path.Combine("ProductImages", fileName);
    }
}