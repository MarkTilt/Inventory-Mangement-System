using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class AddCategoryPopup : Popup
{
    InventoryService inventoryService = new InventoryService();
   

    public AddCategoryPopup()
	{
		InitializeComponent();
       
    }

    private async void AddOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            Category category = new Category(CategoryNameEntry.Text);
            inventoryService.AddCategory(category);
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