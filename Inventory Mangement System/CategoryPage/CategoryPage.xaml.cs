using CommunityToolkit.Maui.Extensions;
using InventorySystem.Core.List;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
namespace Inventory_Mangement_System;

public partial class CategoryPage : ContentPage
{
    InventoryService inventoryService = new InventoryService();
    public CategoryPage()
	{
		InitializeComponent();
        LoadCategories();
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

    private async void DeleteCategory_Clicked(object sender, EventArgs e)
    {
        if(sender is Button button &&
            button.CommandParameter is Category category)
        {
            //Delete confirmation window
            bool delete = await DisplayAlertAsync("Delete Category?", $"Are you sure you want to delete {category.CategoryName}?", "Delete", "Cancel");
            if (delete)
            {
                inventoryService.RemoveCategory(category);
                LoadCategories();
            }
        }
    }

    private async void EditCategory_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
           button.CommandParameter is Category category)
        {
            await this.ShowPopupAsync(new EditCategoryPopup(category, this));
        }
    }
    public void LoadCategories()
    {
        CategoryCollection.ItemsSource = null;
        CategoryCollection.ItemsSource = CategoryList.Categories;
    }
}