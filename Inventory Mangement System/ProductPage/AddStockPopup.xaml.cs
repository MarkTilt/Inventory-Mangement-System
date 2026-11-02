using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class AddStockPopup : Popup
{
    private readonly InventoryService _inventoryService;
    
    public Product Product { get; set; }
    public int AddedQuantity { get; set; }

    public AddStockPopup(Product product, InventoryService inventoryService)
	{
		InitializeComponent();
        Product = product;
        _inventoryService = inventoryService;

        BindingContext = this;
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(QauntityToAdd.Text))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Quantity",
                        "Please enter a quantity to add.", "OK");
                return;
            }

            if (!int.TryParse(QauntityToAdd.Text, out int quantity))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Quantity",
                        "Please enter a valid whole number.", "OK");
                return;
            }
            await _inventoryService.AddStockAsync(Product, quantity);
            await CloseAsync();
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!
                .DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}