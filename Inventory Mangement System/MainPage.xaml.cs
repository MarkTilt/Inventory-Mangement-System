using CommunityToolkit.Maui.Extensions;
using InventorySystem.Core.Data;
using InventorySystem.Core.Services;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Mangement_System

{
    public partial class MainPage : ContentPage
    {
        private readonly InventoryService _inventoryService;
        private readonly InventoryDbContext _context;
        
        
        public MainPage(InventoryService inventoryService, ActivityService activityService, InventoryDbContext context)
        {
            _inventoryService = inventoryService;
            _context = context;

            InitializeComponent();
            
           
        }
        //Updates counts on dash
        public async Task AddCountToDash()
        {
            ProductsCount.Text = null;
            ProductsCount.Text =(await _context.Products.CountAsync()).ToString();
            int lowCount =  await _inventoryService.GetLowStockCountAsync();
            LowCount.Text = null;
            LowCount.Text =  lowCount.ToString();
            CategoriesCount.Text = null;
            CategoriesCount.Text = (await _context.Categories.CountAsync()).ToString();
        }
        public async Task LoadRecentActivities()
        {
            var recentProducts = await _context.Activities.OrderByDescending(p => p.Time).Take(4).ToListAsync();
            RecentActivitiesView.ItemsSource = null;
            RecentActivitiesView.ItemsSource = recentProducts;
            

        }

        private async void Products_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProductPage));
        }

        private async void Categories_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CategoryPage));
        }

        private async void DashBoard_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///MainPage");
        }

        private async void AddProduct_Clicked(object sender, EventArgs e)
        {
            await this.ShowPopupAsync(new AddProductPopupPage(_inventoryService, _context));
            await AddCountToDash();
            await LoadRecentActivities();
        }

        private async void AddCategory_Clicked(object sender, EventArgs e)
        {
            await this.ShowPopupAsync(new AddCategoryPopup(_inventoryService));
            await AddCountToDash();
            await LoadRecentActivities();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AddCountToDash();
            await LoadRecentActivities();

        }
    }
    }

