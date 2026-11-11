using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using InventorySystem.Core.Data;
using Microsoft.EntityFrameworkCore;


namespace Inventory_Mangement_System;

public partial class EditProductPopup : Popup
{
    private readonly InventoryService _inventoryService;
    private readonly IDbContextFactory<InventoryDbContext> _contextFactory;
    private string _originalImagePath;
    private readonly List<string> _newImagePaths = new();
    public string EditedImagePath { get; set; }

    // The original product
    public Product Product { get; set; }

    // Temporary copy for editing
    public string EditedName { get; set; }
    public Category? EditedCategory { get; set; }
    public string EditedPrice { get; set; }
    public string EditedQuantity { get; set; }
    public string EditedLowStockThreshold { get; set; }

    public List<Category> Categories { get; set; } = new();

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

        _originalImagePath = product.ImagePath;
        EditedImagePath = product.ImagePath;


        BindingContext = this;
    }

    private async void SaveOn_Clicked(object? sender, EventArgs e)
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

            if (EditedCategory == null)
            {
                return;
            }

            await _inventoryService.UpdateProductAsync(
                Product,
                EditedName,
                EditedCategory,
                price,
                quantity,
                threshold,
                EditedImagePath);

            await DeleteImageIfUnusedAsync(_originalImagePath);

            foreach (string imagePath in _newImagePaths)
            {
                if (imagePath != EditedImagePath)
                {
                    await DeleteImageIfUnusedAsync(imagePath);
                }
            }
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
        foreach (string imagePath in _newImagePaths)
        {
            await DeleteImageIfUnusedAsync(imagePath);
        }
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

    private async void EditImage_Clicked(object? sender, EventArgs e)
    {
        var fileResult = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image",
            FileTypes = FilePickerFileType.Images
        });
        if (fileResult == null)
        {
            return;
        }
        EditedImagePath = await SaveProductImageAsync(fileResult);
        _newImagePaths.Add(EditedImagePath);

        var fullPath = Path.Combine(FileSystem.AppDataDirectory, EditedImagePath);
        ProductImage.Source = ImageSource.FromFile(fullPath);
    }

    private void ResetImage_Clicked(object? sender, EventArgs e)
    {
        EditedImagePath = "product_placeholder.png";
        ProductImage.Source = EditedImagePath;
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
    private async Task DeleteImageIfUnusedAsync(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || 
            imagePath == "product_placeholder.png")
        {
            return;
        }

        string imagesDirectory = Path.GetFullPath(
            Path.Combine(FileSystem.AppDataDirectory, "ProductImages"));

        string fullPath = Path.GetFullPath(
            Path.Combine(FileSystem.AppDataDirectory, imagePath));

        // Only allow deletion inside ProductImages
        if (!fullPath.StartsWith(
            imagesDirectory + Path.DirectorySeparatorChar,
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await using var context = await _contextFactory.CreateDbContextAsync();

        bool isUsed = await context.Products
      .AnyAsync(p => p.ImagePath == imagePath);

        if (!isUsed && File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}