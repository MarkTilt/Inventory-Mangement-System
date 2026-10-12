using InventorySystem.Core.List;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class ProductPage : ContentPage
{
    InventoryService inventoryService = new InventoryService();
	public ProductPage()
	{
		InitializeComponent();
         LoadProducts();
    }

    private async void DashBoard_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void Products_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage());
    }

    private async void Categories_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CategoryPage());
    }

    private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Product product)
        {
         bool delete  = await DisplayAlertAsync("Delete Product?", $"Are you sure you want to delete {product.ProductName}?", "Delete", "Cancel");
            if (delete)
            {
                inventoryService.RemoveProduct(product);
                LoadProducts();
            }
        }
    }

    private void EditProduct_Clicked(object sender, EventArgs e)
    {

    }
    private void LoadProducts()
    {
        ProductsCollection.ItemsSource = null;
        ProductsCollection.ItemsSource = ProductList.Products;
    }
}