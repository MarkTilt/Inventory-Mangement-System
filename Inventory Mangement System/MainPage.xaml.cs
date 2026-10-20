using CommunityToolkit.Maui.Extensions;
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
            LoadRecentActivities();
            AddCountToDash();
           
        }
        //Updates counts on dash
        public void AddCountToDash()
        {
            ProductsCount.Text = null;
            ProductsCount.Text = ProductList.Products.Count.ToString();
            int lowCount = inventoryService.LowStockCount;
            LowCount.Text = null;
            LowCount.Text =  lowCount.ToString();
            CategoriesCount.Text = null;
            CategoriesCount.Text = CategoryList.Categories.Count.ToString();
        }
        public void LoadRecentActivities()
        {
            var recentProducts = ActivityList.Activities.OrderByDescending(p => p.Time).Take(4).ToList();
            RecentActivitiesView.ItemsSource = null;
            RecentActivitiesView.ItemsSource = recentProducts;
            

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

        private async void AddProduct_Clicked(object sender, EventArgs e)
        {
            await this.ShowPopupAsync(new AddProductPopupPage());
            AddCountToDash();
            LoadRecentActivities();
        }

        private async void AddCategory_Clicked(object sender, EventArgs e)
        {
            await this.ShowPopupAsync(new AddCategoryPopup());
            AddCountToDash();
            LoadRecentActivities();
        }

        }
    }

