using CafeApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeApp.Data.EntityConfigs;

public class TableEntityConfiguration : IEntityTypeConfiguration<TableEntity>
{
    public void Configure(EntityTypeBuilder<TableEntity> builder)
    {
        builder.ToTable("Tables");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Number)
            .IsRequired();

        builder.Property(t => t.Seats)
            .IsRequired()
            .HasDefaultValue(2);

        builder.Property(t => t.ReservedUntil)
            .IsRequired(false);

        builder.HasOne(t => t.Cafe)
            .WithMany(c => c.Tables)
            .HasForeignKey(t => t.CafeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}