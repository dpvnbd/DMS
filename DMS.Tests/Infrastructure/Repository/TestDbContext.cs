using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Tests.Infrastructure.Repository
{
  class TestDbContext : DbContext
  {
    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }
  }
}
