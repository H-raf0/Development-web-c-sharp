using Microsoft.EntityFrameworkCore;
namespace GameServerApi.Models;

public class ProgressionContext : DbContext
{
    public ProgressionContext(DbContextOptions<ProgressionContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Connexion a la base sqlite
        options.UseSqlite("Data Source=Progression.db");
    }

    public DbSet<Progression> Progressions { get; set; } = null!;
}
