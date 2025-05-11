using CafeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeApp.Data.EntityConfigs;

public class CafeEntityConfiguration : IEntityTypeConfiguration<CafeEntity>
{
    public void Configure(EntityTypeBuilder<CafeEntity> builder)
    {
        builder.ToTable("Cafes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Street)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Rating)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(c => c.RatingCount).HasAnnotation("Default", 1);

        builder.Property(c => c.OpeningTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(c => c.ClosingTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(c => c.Latitude)
            .IsRequired()
            .HasColumnType("decimal(9,6)");

        builder.Property(c => c.Longitude)
            .IsRequired()
            .HasColumnType("decimal(9,6)");

        builder.Property(c => c.CreatedAt)
            .IsRequired();
        
        builder.HasMany(c => c.Tables)
            .WithOne(t => t.Cafe)
            .HasForeignKey(t => t.CafeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Dishes)
            .WithOne(d => d.Cafe)
            .HasForeignKey(d => d.CafeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}