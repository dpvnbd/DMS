using DMS.Domain.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DMS.Infrastructure.Repositories
{
  public class MongoRepository<T> : IRepository<T> where T : BaseEntity
  {
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<T> collection;

    public MongoRepository(IMongoDatabase database)
    {
      this.database = database;
      collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task Create(T entity)
    {
      // Set protected Id property to next value in sequence
      var type = entity.GetType();
      var prop = type.GetProperty("Id");
      var nextId = await GetNextSequenceValue();
      prop.SetValue(entity, nextId, null);

      await collection.InsertOneAsync(entity);
      
    }

    public async Task Delete(int id)
    {
      await collection.DeleteOneAsync(x => x.Id == id);
    }

    public IQueryable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
      return collection.AsQueryable().Where(predicate);
    }

    public IQueryable<T> GetAll()
    {
      return collection.AsQueryable();
    }

    public async Task<T> GetById(int id)
    {
      return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task Update(T entity)
    {
      await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new UpdateOptions
      {
        IsUpsert = false
      });
    }

    private async Task<int> GetNextSequenceValue()
    {
      var countersCollection = database.GetCollection<SequenceCounter>("counters");
      var counter = await countersCollection.Find(x => x.Entity == typeof(T).Name).FirstOrDefaultAsync();
      if (counter == null)
      {
        counter = new SequenceCounter
        {
          Entity = typeof(T).Name,
          SequenceValue = 1
        };
      }

      var count = counter.SequenceValue++;
      
      await countersCollection.ReplaceOneAsync(x => x.Entity == typeof(T).Name, counter, new UpdateOptions
      {
        IsUpsert = true
      });
      return count;
    }

    private class SequenceCounter
    {
      public string Entity { get; set; }
      public int SequenceValue { get; set; }
    }
  }
}
