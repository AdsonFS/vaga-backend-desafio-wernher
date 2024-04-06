using Microsoft.EntityFrameworkCore;

namespace Wernher.Unit.Test;

public class TestWernherContext : DbContext
{
    public TestWernherContext(DbContextOptions<TestWernherContext> options) : base(options) { }

    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestWernherContext).Assembly);
}

