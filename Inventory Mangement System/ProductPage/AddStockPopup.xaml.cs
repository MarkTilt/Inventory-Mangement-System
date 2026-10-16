using CommunityToolkit.Maui.Views;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;

namespace Inventory_Mangement_System;

public partial class AddStockPopup : Popup
{
    InventoryService inventoryService = new InventoryService();
    private readonly ProductPage _productPage;
    public Product Product { get; set; }
    public int AddedQuantity { get; set; }

    public AddStockPopup(Product product, ProductPage page)
	{
		InitializeComponent();
        Product = product;
        _productPage = page;

        BindingContext = this;
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        try
        {
            inventoryService.AddStock(Product, int.Parse(QauntityToAdd.Text));
            _productPage.LoadProducts();
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