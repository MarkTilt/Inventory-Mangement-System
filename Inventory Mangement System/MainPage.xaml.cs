using InventorySystem.Core.List;
using InventorySystem.Core.Services;
namespace Inventory_Mangement_System

{
    public partial class MainPage : ContentPage
    {
        InventoryService inventoryService = new InventoryService();
        
        public MainPage()
        {

            InitializeComponent();
            AddCountToDash();
           
        }
        //Updates counts on dash
        public void AddCountToDash()
        {
            ProductsCount.Text = ProductList.Products.Count.ToString();
            int lowCount = inventoryService.LowStockCount;
            LowCount.Text =  lowCount.ToString();
            CategoriesCount.Text = CategoryList.Categories.Count.ToString();
        }

        private async void Products_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage());
        }

        private async void Categories_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoryPage());
        }

        private async void DashBoard_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
