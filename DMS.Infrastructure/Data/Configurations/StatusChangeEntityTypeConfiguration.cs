using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class StatusChangeEntityTypeConfiguration : IEntityTypeConfiguration<StatusChange>
  {
    public void Configure(EntityTypeBuilder<StatusChange> builder)
    {
      builder.HasKey(s => s.Id);
      builder.HasOne(s => s.Document)
        .WithMany(d => d.StatusChanges);
    }
  }
}
