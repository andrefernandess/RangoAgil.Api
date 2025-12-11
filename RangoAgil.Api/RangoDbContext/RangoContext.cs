using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.Entities;

namespace RangoAgil.Api.RangoDbContext;

public class RangoContext(DbContextOptions<RangoContext> options) : DbContext(options)
{
    public DbSet<Rango> Rangos { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>();

        modelBuilder.Entity<Rango>();

        modelBuilder.Entity<Rango>()
            .HasMany(r => r.Ingredients)
            .WithMany(i => i.Rangos);

        base.OnModelCreating(modelBuilder);
    }
}
