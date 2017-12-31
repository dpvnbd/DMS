using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Abstract
{
  public interface IRepository<T>
  {
    IQueryable<T> GetAll();

    Task<T> GetById(int id);

    Task Create(T entity);

    Task Update(int id, T entity);

    Task Delete(int id);
  }
}
