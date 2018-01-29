using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DMS.Domain.Abstract.Services;
using DMS.Domain.Entities;
using DMS.Domain.Abstract;

namespace DMS.Domain.Services
{
  public class DocumentFilteringService : IDocumentFilteringService
  {
    
    public IQueryable<Document> Filter(IQueryable<Document> documents, AppUser author = null, DocumentStatus? status = null,
      string searchString = null, AppUser canBeReviewedBy = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
      bool predicate(Document d)
      {
        if (author != null && d.Author.Id != author.Id)
        {
          return false;
        }

        if (status.HasValue && d.CurrentStatus != status.Value)
        {
          return false;
        }

        if (fromDate.HasValue && d.Created < fromDate.Value)
        {
          return false;
        }

        if (toDate.HasValue && d.Created > toDate.Value)
        {
          return false;
        }

        if (!string.IsNullOrWhiteSpace(searchString))
        {
          if (d.Title.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) < 0
          && d.Title.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) < 0)
          {
            return false;
          }
        }

        if (canBeReviewedBy != null)
        {
          var availableChanges = d.AvailableStatusChanges(canBeReviewedBy);
          if(!availableChanges.Any() && !d.CanEdit(canBeReviewedBy))
          {
            return false;
          }
        }

        return true;
      }

      return documents.Where(predicate).AsQueryable();
    }
  }
}
