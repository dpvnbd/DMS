using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DMS.Domain.Entities;

namespace DMS.Domain.Abstract.Services
{
  public interface IDocumentFilteringService
  {
     IQueryable<Document> Filter(IQueryable<Document> documents, AppUser author = null, DocumentStatus? status = null,
      string searchString = null, AppUser canBeReviewedBy = null, DateTime? fromDate = null, DateTime? toDate = null);
  }
}
