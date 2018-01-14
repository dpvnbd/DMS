using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Configurations;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data
{
  public class AppDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
  {
    public DbSet<Document> Documents { get; set; }
    public DbSet<AppUser> DomainUsers { get; set; }
    public DbSet<DocumentHistoryEntry> StatusChanges { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new DocumentEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new DocumentHistoryEntryEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new AppIdentityUserEntityTypeConfiguration());
    }
  }
}
