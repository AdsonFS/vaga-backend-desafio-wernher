using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wernher.Domain.Models;

namespace Wernher.Data.Configurations;
public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Identifier).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Description).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Manufacturer).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Url).IsRequired().HasMaxLength(100);

        builder.HasMany(d => d.Commands)
            .WithOne()
            .HasForeignKey(c => c.DeviceID)
            .IsRequired();
    }
}

public class CommandDescriptionConfiguration : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Operation).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Result).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Format).IsRequired().HasMaxLength(100);

        builder.HasOne(cd => cd.TelnetCommand)
            .WithOne()
            .HasForeignKey<Command>(cd => cd.Id)
            .IsRequired();
    }
}

public class TelnetCommandConfiguration : IEntityTypeConfiguration<TelnetCommand>
{
    public void Configure(EntityTypeBuilder<TelnetCommand> builder)
    {
        builder.HasKey(tc => tc.Id);
        builder.Property(tc => tc.Command).IsRequired().HasMaxLength(50);

        builder.HasMany(tc => tc.Parameters)
            .WithOne()
            .HasForeignKey(p => p.CommandID)
            .IsRequired();
    }
}

public class ParameterConfiguration : IEntityTypeConfiguration<Parameter>
{
    public void Configure(EntityTypeBuilder<Parameter> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(50);
    }
}