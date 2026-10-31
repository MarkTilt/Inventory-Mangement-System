
using InventorySystem.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventorySystem.Migrations;

public class InventoryDbContextFactory
    : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("INVENTORY_DB_CONNECTION")
            ?? throw new InvalidOperationException(
                "Database connection string is not configured.");

        var options = new DbContextOptionsBuilder<InventoryDbContext>()
               .UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly("InventorySystem.Migrations"))
               .Options;

        return new InventoryDbContext(options);
    }
}
