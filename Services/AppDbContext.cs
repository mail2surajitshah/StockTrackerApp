using Microsoft.EntityFrameworkCore;
using StockTrackerApp.Contracts;

namespace StockTrackerApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<StockPriceEntity> StockPrices => Set<StockPriceEntity>();
}

