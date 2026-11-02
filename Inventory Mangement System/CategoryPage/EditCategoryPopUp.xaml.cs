using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class EditCategoryPopup : Popup
{
    public Category Category { get; set; }
    private readonly InventoryService _inventoryService;
    public string EditedCategoryName { get; set; }
	
    public EditCategoryPopup(Category category, InventoryService inventoryService)
	{
		InitializeComponent();
        Category = category;
        EditedCategoryName = category.CategoryName;
        _inventoryService = inventoryService;

        BindingContext = this;

    }

    private async void SaveOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await _inventoryService.UpdateCategoryAsync(Category, EditedCategoryName);
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
}