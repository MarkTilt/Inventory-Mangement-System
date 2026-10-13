using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
namespace Inventory_Mangement_System;

public partial class EditProductPopup : Popup
{
	
    public EditProductPopup(Product product)
	{
		InitializeComponent();
		BindingContext = product;
	
	}

    private void SaveOn_Clicked(object sender, EventArgs e)
    {
		
    }

    private void CancelOn_Clicked(object sender, EventArgs e)
    {

    }
}