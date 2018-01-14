using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class DocumentHistoryEntryEntityTypeConfiguration : IEntityTypeConfiguration<DocumentHistoryEntry>
  {
    public void Configure(EntityTypeBuilder<DocumentHistoryEntry> builder)
    {
      builder.HasKey(s => s.Id);
      builder.HasOne(s => s.Document)
        .WithMany(d => d.History);

      builder.HasOne(s => s.User).WithMany().IsRequired();

      builder.HasOne(s => s.OnBehalfOfUser).WithMany();
    }
  }
}
