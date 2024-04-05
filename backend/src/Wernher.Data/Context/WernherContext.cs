using Microsoft.EntityFrameworkCore;
using Wernher.Domain.Models;

namespace Wernher.Data.Context;

public class WernherContext : DbContext
{
    public WernherContext(DbContextOptions<WernherContext> options) : base(options) { }

    public DbSet<Device> Devices { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<TelnetCommand> TelnetCommands { get; set; }
    public DbSet<Command> CommandDescriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(WernherContext).Assembly);
}
