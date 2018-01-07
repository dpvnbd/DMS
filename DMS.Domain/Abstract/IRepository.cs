using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Abstract
{
  public interface IRepository<T>
  {
    IQueryable<T> GetAll();
    
    Task<T> GetById(int id);

    IQueryable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    
    Task Create(T entity);

    Task Update(T entity);

    Task Delete(int id);
  }
}
