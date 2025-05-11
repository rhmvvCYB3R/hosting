using System.Reflection;
using CafeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Data;

public class CafeAppDbContext : DbContext
{
    public DbSet<CafeEntity> Cafes { get; set; }
    public DbSet<DishEntity> Dishes { get; set; }
    public DbSet<TableEntity> Tables { get; set; }
    
    public CafeAppDbContext(DbContextOptions<CafeAppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}