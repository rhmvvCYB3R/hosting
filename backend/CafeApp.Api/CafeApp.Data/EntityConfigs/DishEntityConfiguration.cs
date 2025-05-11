using CafeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeApp.Data.EntityConfigs;

public class DishEntityConfiguration : IEntityTypeConfiguration<DishEntity>
{
    public void Configure(EntityTypeBuilder<DishEntity> builder)
    {
        builder.ToTable("Dishes");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(d => d.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.HasOne(d => d.Cafe)
            .WithMany(c => c.Dishes)
            .HasForeignKey(d => d.CafeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
