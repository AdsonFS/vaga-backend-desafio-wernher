using Microsoft.EntityFrameworkCore;
using Wernher.Domain.Models;

namespace Wernher.Data.Context;

public class WernherContext : DbContext
{
    public WernherContext(DbContextOptions<WernherContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(WernherContext).Assembly);
}
