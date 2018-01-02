using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
  {
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
      builder.HasKey(u => u.Id);
    }
  }
}
