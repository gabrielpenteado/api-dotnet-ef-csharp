using api_dotnet_ef_csharp.Ebooks;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet_ef_csharp.Database;

public class AppDbContext : DbContext
{
    public DbSet<Ebook> Ebooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source = db.sqlite");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        base.OnConfiguring(optionsBuilder);
    }
}

