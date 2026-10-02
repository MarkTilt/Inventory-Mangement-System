using InventorySystem.Core.List;
namespace Inventory_Mangement_System

{
    public partial class MainPage : ContentPage
    {
     

        public MainPage()
        {
           
            InitializeComponent();
            AddCountToDash();

        }
        public void AddCountToDash()
        {
            ProductsCount.Text = ProductList.Products.Count.ToString();
            //LowCount.Text
            CategoriesCount.Text = CategoryList.Categories.Count.ToString();
        }
    
    }
}
