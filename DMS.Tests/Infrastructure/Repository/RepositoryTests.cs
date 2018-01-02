using DMS.Infrastructure;
using DMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Infrastructure.Repository
{
  [TestClass]
  public class RepositoryTests
  {
    private void InitializeContext(TestDbContext context)
    {
      context.Add(new TestEntity { TestProperty = "first" });
      context.Add(new TestEntity { TestProperty = "second" });
      context.Add(new TestEntity { TestProperty = "third" });
      context.SaveChanges();
    }

    [TestMethod]
    public async Task Create_writes_to_database()
    {
      // Arrange
      var builder = new DbContextOptionsBuilder<TestDbContext>()
        .UseInMemoryDatabase(databaseName: "Create_db");

      // Act
      using (var context = new TestDbContext(builder.Options))
      {
        var repo = new Repository<TestEntity>(context);
        await repo.Create(new TestEntity { TestProperty = "Created" });
      }

      // Assert
      using (var context = new TestDbContext(builder.Options))
      {
        var repo = new Repository<TestEntity>(context);
        var entity = await repo.GetAll().SingleOrDefaultAsync();
        Assert.IsNotNull(entity);
        Assert.AreEqual("Created", entity.TestProperty);
      }
    }

    [TestMethod]
    public async Task GetAll_returns_all_entities()
    {
      // Arrange
      var builder = new DbContextOptionsBuilder<TestDbContext>()
        .UseInMemoryDatabase(databaseName: "GetAll_db");
      var initContext = new TestDbContext(builder.Options);
      InitializeContext(initContext);
      // Act
      List<TestEntity> results;
      using (var context = new TestDbContext(builder.Options))
      {
        var repo = new Repository<TestEntity>(context);
        results = await repo.GetAll().ToListAsync();
      }

      // Assert
      Assert.AreEqual(3, results.Count);
      Assert.AreEqual("second", results[1].TestProperty);
    }

    [TestMethod]
    public async Task Delete_removes_from_database()
    {
      // Arrange
      var builder = new DbContextOptionsBuilder<TestDbContext>()
        .UseInMemoryDatabase(databaseName: "Remove_db");
      var initContext = new TestDbContext(builder.Options);
      InitializeContext(initContext);

      // Act
      using (var context = new TestDbContext(builder.Options))
      {
        var repo = new Repository<TestEntity>(context);
        var entity = await repo.GetAll().SingleOrDefaultAsync(e => e.TestProperty == "second");
        await repo.Delete(entity.Id);
      }

      // Assert
      using (var context = new TestDbContext(builder.Options))
      {
        var repo = new Repository<TestEntity>(context);
        var entities = await repo.GetAll().ToListAsync();
        var entity = await repo.GetAll().SingleOrDefaultAsync(e => e.TestProperty == "second");
        Assert.AreEqual(2, entities.Count);
        Assert.IsNull(entity);
      }
    }
  }
}
