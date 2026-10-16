using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class RemoveStockPopup : Popup
{
    InventoryService inventoryService = new InventoryService();
    private readonly ProductPage _productPage;
    public Product Product { get; set; }
    public int AddedQuantity { get; set; }
    public RemoveStockPopup(Product product, ProductPage page)
	{
		InitializeComponent();
        Product = product;
        _productPage = page;
        BindingContext = this;
    }

    private async void Remove_Clicked(object sender, EventArgs e)
    {
        try
        {
            inventoryService.RemoveStock(Product, int.Parse(QauntityToRemove.Text));
            _productPage.LoadProducts();
            await CloseAsync();
        }
        catch(Exception ex)
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