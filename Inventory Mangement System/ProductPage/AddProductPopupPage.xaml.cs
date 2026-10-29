using InventorySystem.Core.List;
using InventorySystem.Core.Models;
using InventorySystem.Core.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;
namespace Inventory_Mangement_System;

public partial class AddProductPopupPage : Popup
{

    InventoryService inventoryService = new InventoryService();
   
    public List<Category> Categories { get; set; }
    public AddProductPopupPage()
	{
		InitializeComponent();
        Categories = CategoryList.Categories;
        BindingContext = this;
    }

    private async void AddOn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!decimal.TryParse(ProductPrice.Text, out decimal price))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Price",
                        "Please enter a valid price.", "OK");
                return;
            }

            if (!int.TryParse(ProductQauntity.Text, out int quantity))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Quantity",
                        "Please enter a valid whole number.", "OK");
                return;
            }

            if (!int.TryParse(ProductLowStockThreshold.Text, out int threshold))
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Threshold",
                        "Please enter a valid whole number.", "OK");
                return;
            }

            if (ProductCategory.SelectedItem is not Category category)
            {
                await Application.Current!.Windows[0].Page!
                    .DisplayAlertAsync("Invalid Category",
                        "Please select a category.", "OK");
                return;
            }

            Product product = new Product(
                ProductName.Text,
                category,
                price,
                quantity,
                threshold);

            inventoryService.AddProduct(product);
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