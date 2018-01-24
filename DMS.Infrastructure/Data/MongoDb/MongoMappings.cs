using DMS.Domain.Entities;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.MongoDb
{
  public static class MongoMappings
  {
    public static void SetupMappings()
    {
      BsonClassMap.RegisterClassMap<AppUser>();
      BsonClassMap.RegisterClassMap<Document>(cm =>
      {
        cm.AutoMap();
        cm.MapField("history").SetElementName("History");

        cm.UnmapProperty(c => c.LastHistoryEntry);
      });

      BsonClassMap.RegisterClassMap<DocumentHistoryEntry>();
    }
  }
}
