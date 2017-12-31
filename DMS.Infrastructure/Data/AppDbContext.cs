using System;
using System.Collections.Generic;
using System.Text;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<Document> Documents { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<StatusChange> StatusChanges { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new DocumentEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new StatusChangeEntityTypeConfiguration());
    }
  }
}
