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
  public class DocumentRepository : Repository<Document>
  {
    public DocumentRepository(AppDbContext context) : base(context)
    {
    }  

    public override IQueryable<Document> GetAll()
    {
      return base.GetAll().Include(d => d.Author).Include(d => d.StatusChanges).ThenInclude(s => s.ChangeAuthor);
    }   
  }
}
