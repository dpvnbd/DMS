using DMS.Domain.Abstract;
using DMS.Domain.Entities;
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
  public class DocumentRepository : Repository<Document>,  IDocumentRepository
  {
    public DocumentRepository(AppDbContext context) : base(context)
    {
    }

    public IQueryable<Document> FindBy(Expression<Func<Document, bool>> predicate)
    {
      return GetAll().Where(predicate);
    }

    public new IQueryable<Document> GetAll()
    {
      return base.GetAll().Include(d => d.Author).Include(d => d.StatusChanges).ThenInclude(s => s.ChangeAuthor);
    }

    public new Task<Document> GetById(int id)
    {
      return FindBy(d => d.Id == id).FirstOrDefaultAsync();
    }
  }
}
