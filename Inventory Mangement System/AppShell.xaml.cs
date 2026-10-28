namespace Inventory_Mangement_System
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
        }
    }
}
