using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class EditCategoryPopup : Popup
{
    public Category Category { get; set; }
	InventoryService inventoryService = new InventoryService();
    public string EditedCategoryName { get; set; }
	private readonly CategoryPage _categoryPage;
    public EditCategoryPopup(Category category, CategoryPage page)
	{
		InitializeComponent();
        Category = category;
        EditedCategoryName = category.CategoryName;
        _categoryPage = page;

        BindingContext = this;

    }

    private async void SaveOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            inventoryService.UpdateCategory(Category, EditedCategoryName);
            await CloseAsync();
            _categoryPage.LoadCategories();
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