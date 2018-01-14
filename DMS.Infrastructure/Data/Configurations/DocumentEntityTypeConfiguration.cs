using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Configurations
{
  class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
  {
    public void Configure(EntityTypeBuilder<Document> documentConfiguration)
    {
      documentConfiguration.HasKey(d => d.Id);

      var navigation = documentConfiguration.Metadata.FindNavigation(nameof(Document.History));
      navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

      documentConfiguration.HasMany(d => d.History)
        .WithOne(s => s.Document);

      documentConfiguration.HasOne(d => d.Author).WithMany();
    }
  }
}
