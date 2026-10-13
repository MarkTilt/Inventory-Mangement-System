using InventorySystem.Core.List;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;
namespace Inventory_Mangement_System;

public partial class AddProductPopupPage : Popup
{

    InventoryService inventoryService = new InventoryService();
    private readonly ProductPage _productPage;
    public List<Category> Categories { get; set; }
    public AddProductPopupPage(ProductPage page)
	{
		InitializeComponent();
        Categories = CategoryList.Categories;
        _productPage = page;
        BindingContext = this;
    }

    private async void AddOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            Product product = new Product(ProductName.Text, 
                (Category)ProductCategory.SelectedItem, 
                decimal.Parse(ProductPrice.Text), 
                int.Parse(ProductQauntity.Text), 
                int.Parse(ProductLowStockThreshold.Text));

            inventoryService.AddProduct(product);
            _productPage.LoadProducts();
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