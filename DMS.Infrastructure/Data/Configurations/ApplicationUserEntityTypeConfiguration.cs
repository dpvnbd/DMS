using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
  {
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
      builder.HasKey(u => u.Id);
    }
  }
}
