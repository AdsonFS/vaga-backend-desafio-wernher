using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wernher.Domain.Models;

namespace Wernher.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);

        builder.HasIndex(d => d.Email).IsUnique();
        builder.Property(d => d.Email).IsRequired().HasMaxLength(100);

        builder.Property(d => d.Password).IsRequired().HasMaxLength(100);

        builder.HasMany(d => d.Devices)
            .WithOne()
            .HasForeignKey(c => c.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
