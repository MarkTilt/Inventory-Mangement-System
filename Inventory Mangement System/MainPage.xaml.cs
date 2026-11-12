using CommunityToolkit.Maui.Extensions;
using InventorySystem.Core.Data;
using InventorySystem.Core.Services;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Mangement_System

{
    public partial class MainPage : ContentPage
    {
        private readonly InventoryService _inventoryService;
        private readonly IDbContextFactory<InventoryDbContext> _contextFactory;


        public MainPage(InventoryService inventoryService, ActivityService activityService, IDbContextFactory<InventoryDbContext> contextFactory)
        {
            InitializeComponent();
            _inventoryService = inventoryService;
            _contextFactory = contextFactory;

        }
        //Updates counts on dash
        public async Task AddCountToDash()
        {

            await using var context = await _contextFactory.CreateDbContextAsync();

            ProductsCount.Text = null;
            ProductsCount.Text =(await context.Products.CountAsync()).ToString();
            LowCount.Text = (await context.Products.CountAsync(p => p.Quantity <= p.LowStockThreshold)).ToString();
            CategoriesCount.Text = null;
            CategoriesCount.Text = (await context.Categories.CountAsync()).ToString();
            OutOfStockCount.Text = (await context.Products.CountAsync(p => p.Quantity == 0)).ToString();
        }
        public async Task LoadRecentActivities()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var recentProducts = await context.Activities.OrderByDescending(p => p.Time).Take(4).ToListAsync();
            RecentActivitiesView.ItemsSource = null;
            RecentActivitiesView.ItemsSource = recentProducts;
            

        }

        private async void Products_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProductPage));
        }

        private async void Categories_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CategoryPage));
        }

        private async void DashBoard_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///MainPage");
        }

        private async void AddProduct_Clicked(object? sender, EventArgs e)
        {
            await this.ShowPopupAsync(new AddProductPopupPage(_inventoryService, _contextFactory));
            await AddCountToDash();
            await LoadRecentActivities();
        }

        private async void AddCategory_Clicked(object? sender, EventArgs e)
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

