using CommunityToolkit.Maui.Extensions;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Data;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Mangement_System;

public partial class CategoryPage : ContentPage
{
    private readonly InventoryService _inventoryService;
    private readonly IDbContextFactory<InventoryDbContext> _contextFactory;
    private bool _isLoadingCategories = false;

    public CategoryPage(InventoryService inventoryService, IDbContextFactory<InventoryDbContext> contextFactory)
	{
		_inventoryService = inventoryService;
		_contextFactory = contextFactory;

		InitializeComponent();
       
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

    private async void DeleteCategory_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.CommandParameter is Category category)
        {
            try
            {
                //Delete confirmation window
                bool delete = await DisplayAlertAsync("Delete Category?", $"Are you sure you want to delete {category.CategoryName}?", "Delete", "Cancel");
                if (delete)
                {
                    await _inventoryService.RemoveCategoryAsync(category);
                    await LoadCategories();
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows[0].Page!
                   .DisplayAlertAsync("Error", ex.Message, "OK");

            }
        }
    }

    private async void EditCategory_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
           button.CommandParameter is Category category)
        {
            await this.ShowPopupAsync(new EditCategoryPopup(category, _inventoryService));
            await LoadCategories();
        }
    }
    public async Task LoadCategories()
    {
        try
        {
            _isLoadingCategories = true;

            CategoryCollection.ItemsSource = null;
            CategorySearchBar.Text = string.Empty;

            await using var context = await _contextFactory.CreateDbContextAsync();
            CategoryCollection.ItemsSource = await context.Categories.ToListAsync();
        }
        finally
        {
            _isLoadingCategories = false;
        }
        

        

    }

    private async void AddCategory_Clicked(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new AddCategoryPopup(_inventoryService));
        await LoadCategories();
    }

    private async void CategorySearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(_isLoadingCategories)
        {
            return;
        }
        await using var context = await _contextFactory.CreateDbContextAsync();
        string searchText = e.NewTextValue?.Trim() ?? "";   

        var filteredCategories = await context.Categories.Where(category => category.CategoryName.Contains(searchText))
               .ToListAsync();


        CategoryCollection.ItemsSource = filteredCategories;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCategories();
    }
}