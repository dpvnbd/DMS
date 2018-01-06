using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Abstract
{
  /// <summary>
  /// Provide access to documents including all of their properties
  /// (a workaround for lack of lazy loading in EF)
  /// </summary>
  public interface IDocumentRepository : IRepository<Document>
  {
    new IQueryable<Document> GetAll();

    IQueryable<Document> FindBy(Expression<Func<Document, bool>> predicate);

    new Task<Document> GetById(int id);   
  }
}
