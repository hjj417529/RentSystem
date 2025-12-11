using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<AccountViewModel> account { get; set; } = null!;
    public DbSet<StockViewModel> stock { get; set; } = null!;

}
