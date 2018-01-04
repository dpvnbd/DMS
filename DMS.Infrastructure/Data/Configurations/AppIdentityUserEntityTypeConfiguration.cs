using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class AppIdentityUserEntityTypeConfiguration : IEntityTypeConfiguration<AppIdentityUser>
  {
    public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
    {
      builder.HasOne(u => u.AppUser).WithOne().HasForeignKey<AppIdentityUser>(u => u.AppUserId);
    }
  }
}
