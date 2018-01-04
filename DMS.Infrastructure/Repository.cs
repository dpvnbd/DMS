using DMS.Domain.Abstract;
using DMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
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

    public IQueryable<TEntity> GetAll()
    {
      return _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetById(int id)
    {
      return await _dbContext.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task Create(TEntity entity)
    {
      await _dbContext.Set<TEntity>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
      var entity = await GetById(id);
      _dbContext.Set<TEntity>().Remove(entity);
      await _dbContext.SaveChangesAsync();
    }

    

    public async Task Update(int id, TEntity entity)
    {
      _dbContext.Set<TEntity>().Update(entity);
      await _dbContext.SaveChangesAsync();
    }
  }
}
