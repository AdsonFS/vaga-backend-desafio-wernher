using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wernher.Data.Context;
using Wernher.Data.Repositories;
using Wernher.Domain.Models;
using Xunit;

namespace Wernher.Unit.Test;

public class RepositoryTests : IDisposable
{
    private readonly DbContextOptions<TestWernherContext> _options;
    private readonly TestWernherContext _context;
    private readonly Repository<TestEntity> _repository;

    public RepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TestWernherContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .Options;
        _context = new TestWernherContext(_options);
        _repository = new Repository<TestEntity>(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };

        // Act
        var result = await _repository.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
        await _repository.AddAsync(entity);

        var updatedEntity = new TestEntity { Id = entity.Id, Name = "Updated Entity" };

        // Act
        var result = await _repository.UpdateAsync(entity, updatedEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedEntity, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
        await _repository.AddAsync(entity);

        // Act
        var result = await _repository.DeleteAsync(entity);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var entity = new TestEntity { Id = entityId, Name = "Test Entity" };
        await _repository.AddAsync(entity);

        // Act
        var result = await _repository.GetByIdAsync(entityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity, result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = new List<TestEntity>
            {
                new TestEntity { Id = Guid.NewGuid(), Name = "Entity 1" },
                new TestEntity { Id = Guid.NewGuid(), Name = "Entity 2" },
                new TestEntity { Id = Guid.NewGuid(), Name = "Entity 3" }
            };

        foreach (var entity in entities)
        {
            await _repository.AddAsync(entity);
        }

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(entities.Count, result.Count);
        foreach (var entity in entities)
        {
            Assert.Contains(entity, result);
        }
    }
}

public class TestEntity : Entity
{
    public string Name { get; set; }
}

