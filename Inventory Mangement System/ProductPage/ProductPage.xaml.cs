using InventorySystem.Core.List;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace Inventory_Mangement_System;

public partial class ProductPage : ContentPage
{
    InventoryService inventoryService = new InventoryService();
    public ProductPage()
    {
        InitializeComponent();

        StatusFilter.ItemsSource = new List<string>
    {
        "All Products",
        "In Stock",
        "Low Stock",
        "Out of Stock"
    };
        StatusFilter.SelectedIndex = 0;

        LoadProducts();
    }

    private async void DashBoard_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
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
                inventoryService.RemoveProduct(product);
                LoadProducts();
            }
        }
    }

    private async void EditProduct_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
           button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new EditProductPopup(product, this));
        }
    }
    public void LoadProducts()
    {
        ProductsCollection.ItemsSource = null;
        ProductsCollection.ItemsSource = ProductList.Products;
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new AddProductPopupPage());
        LoadProducts();
    }

    private async void AddStock_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new AddStockPopup(product, this));
        }
    }

    private async void RemoveStock_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
          button.CommandParameter is Product product)
        {
            await this.ShowPopupAsync(new RemoveStockPopup(product, this));
        }
    }

    private void ProductSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void FilterOn_Changed(object sender, EventArgs e)
    {
        ApplyFilters();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadProducts();
    }

    private void ApplyFilters()
    {
        string searchText = ProductSearchBar.Text?.Trim() ?? "";
        string selectedStatus = StatusFilter.SelectedItem?.ToString()
                                ?? "All Products";

        var filteredProducts = ProductList.Products
            .Where(p =>
                p.ProductName.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase)
                ||
                p.Category.CategoryName.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase))
            .Where(p => selectedStatus switch
            {
                "In Stock" => p.Quantity > p.LowStockThreshold,
                "Low Stock" => p.Quantity <= p.LowStockThreshold
                               && p.Quantity > 0,
                "Out of Stock" => p.Quantity == 0,
                _ => true
            })
            .ToList();

        ProductsCollection.ItemsSource = filteredProducts;
    }
}