using DMS.Domain.Abstract;
using DMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repositories
{
  public class Repository<T> : IRepository<T> where T : BaseEntity
  {
    private readonly DbContext _dbContext;

    public Repository(DbContext context)
    {
      _dbContext = context;
    }

    public Repository(AppDbContext context)
    {
      _dbContext = context;
    }

    public virtual IQueryable<T> GetAll()
    {
      return _dbContext.Set<T>();
    } 

    public async Task<T> GetById(int id)
    {
      return await GetAll().FirstOrDefaultAsync(t => t.Id == id);
    }

    public IQueryable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
      var query = GetAll().Where(predicate);
      return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public async Task Create(T entity)
    {
      await _dbContext.Set<T>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
      var entity = await GetById(id);
      _dbContext.Set<T>().Remove(entity);
      await _dbContext.SaveChangesAsync();
    }

    

    public async Task Update(T entity)
    {
      _dbContext.Set<T>().Update(entity);
      await _dbContext.SaveChangesAsync();
    }
  }
}
