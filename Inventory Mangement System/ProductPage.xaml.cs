using InventorySystem.Core.List;

namespace Inventory_Mangement_System;

public partial class ProductPage : ContentPage
{
	public ProductPage()
	{
		InitializeComponent();
        ProductsCollection.ItemsSource = ProductList.Products;
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
}