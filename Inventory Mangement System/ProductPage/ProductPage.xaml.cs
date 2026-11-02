
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui.Extensions;
using InventorySystem.Core.Data;

namespace Inventory_Mangement_System;

public partial class ProductPage : ContentPage
{
    private readonly InventoryService _inventoryService;
    private readonly InventoryDbContext _context;

    public ProductPage(InventoryService inventoryService, InventoryDbContext context)
    {
        _inventoryService = inventoryService;
        _context = context;

        InitializeComponent();

        StatusFilter.ItemsSource = new List<string>
    {
        "All Products",
        "In Stock",
        "Low Stock",
        "Out of Stock"
    };
        StatusFilter.SelectedIndex = 0;

    }

    private async void DashBoard_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private async void Products_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProductPage));
    }

    private async void Categories_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CategoryPage));
    }

    private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Product product)
        {
            //Delete confirmation window
            bool delete = await DisplayAlertAsync("Delete Product?", $"Are you sure you want to delete {product.ProductName}?", "Delete", "Cancel");
            if (delete)
            {
                await _inventoryService.RemoveProductAsync(product);
                await LoadProducts();
            }
        }
    }

    private async void EditProduct_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
           button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new EditProductPopup(product, _inventoryService, _context));
            await LoadProducts();
        }
    }
    public async Task LoadProducts()
    {
        ProductsCollection.ItemsSource = null;
        await ApplyFilters();
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new AddProductPopupPage(_inventoryService, _context));
        await LoadProducts();
    }

    private async void AddStock_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new AddStockPopup(product, _inventoryService));
            await LoadProducts();
        }
    }

    private async void RemoveStock_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
          button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new RemoveStockPopup(product, _inventoryService));
            await LoadProducts();
        }
    }

    private async void ProductSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        await ApplyFilters();
    }

    private async void FilterOn_Changed(object sender, EventArgs e)
    {
        await ApplyFilters();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
    }

    private async Task ApplyFilters()
    {
        string searchText = ProductSearchBar.Text?.Trim() ?? "";
        string selectedStatus = StatusFilter.SelectedItem?.ToString()
                                ?? "All Products";

        var query = _context.Products
            .Include(p => p.Category)
            .Where(p =>
                p.ProductName.Contains(searchText) ||
                p.Category.CategoryName.Contains(searchText));

        query = selectedStatus switch
        {
            "In Stock" => query.Where(p => p.Quantity > p.LowStockThreshold),

            "Low Stock" => query.Where(p =>
                p.Quantity <= p.LowStockThreshold && p.Quantity > 0),

            "Out of Stock" => query.Where(p => p.Quantity == 0),

            _ => query
        };

        var filteredProducts = await query.ToListAsync();

        ProductsCollection.ItemsSource = filteredProducts;
    }
}
