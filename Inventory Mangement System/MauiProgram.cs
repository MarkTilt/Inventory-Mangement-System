using CommunityToolkit.Maui;
using InventorySystem.Core.Data;
using InventorySystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory_Mangement_System;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        string connectionString =
        Environment.GetEnvironmentVariable("INVENTORY_DB_CONNECTION")
        ?? throw new InvalidOperationException(
        "Database connection string is not configured.");

        builder.Services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("InventorySystem.Migrations")));

        builder.Services.AddTransient<ActivityService>();
        builder.Services.AddScoped<InventoryService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
