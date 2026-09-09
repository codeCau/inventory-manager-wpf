using Microsoft.EntityFrameworkCore;
using ControleEstoqueWPF.Models;

namespace ControleEstoqueWPF.Data;

public class AppDbContext : DbContext
{
    public DbSet<Produto> Produtos => Set<Produto>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Preco).HasPrecision(10, 2);
        });
    }
}
