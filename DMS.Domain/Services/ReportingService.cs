using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Abstract.Services;
using DMS.Domain.Entities.Reporting;

namespace DMS.Domain.Services
{
  public class ReportingService : IReportingService
  {
    private readonly IRepository<Document> docRepo;
    private readonly IRepository<DocumentHistoryEntry> historyRepo;

    public ReportingService(IRepository<Document> docRepo, IRepository<DocumentHistoryEntry> historyRepo)
    {
      this.docRepo = docRepo;
      this.historyRepo = historyRepo;
    }

    public IDictionary<DocumentStatus, int> CountDocumentsByStatus(IQueryable<Document> documents)
    {
      var result = new Dictionary<DocumentStatus, int>();
      foreach (DocumentStatus status in Enum.GetValues(typeof(DocumentStatus)))
      {
        result[status] = documents.Count(d => d.CurrentStatus == status);
      }
      return result;
    }   

    public IDictionary<DocumentStatus, int> CountStatusChanges(IQueryable<DocumentHistoryEntry> historyEntries)
    {
      var result = new Dictionary<DocumentStatus, int>();

      foreach (DocumentStatus status in Enum.GetValues(typeof(DocumentStatus)))
      {
        result[status] = historyEntries.Count(e => e.Status.HasValue && e.Status.Value == status);
      }
      return result;
    }   

    public IDictionary<StatusChange, TimeSpan> TimeBetweenStatusChanges(Document document)
    {
      return TimeBetweenStatusChanges(document.History);
    }

    public IDictionary<StatusChange, TimeSpan> TimeBetweenStatusChanges(IEnumerable<DocumentHistoryEntry> history)
    {
      var result = new Dictionary<StatusChange, TimeSpan>();
      DocumentHistoryEntry previousEntry = null;
      foreach (var nextEntry in history)
      {
        if (previousEntry != null && previousEntry.Status.HasValue)
        {
          if (nextEntry.Status.HasValue)
          {
            var change = new StatusChange(previousEntry.Status.Value, nextEntry.Status.Value);
            var time = nextEntry.Created - previousEntry.Created;
            
            if (!result.ContainsKey(change))
            {
              result[change] = time;
            }
            else
            {
              result[change] += time;
            }
          }
          else
          {
            continue;
          }
        }
        previousEntry = nextEntry;
      }
      return result;
    }

    public IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(
      IQueryable<IEnumerable<DocumentHistoryEntry>> documentHistories)
    {
      var result = new Dictionary<StatusChange, TimeSpan>();
      var count = new Dictionary<StatusChange, int>();

      foreach (var history in documentHistories)
      {
        var documentTime = TimeBetweenStatusChanges(history);
        foreach (var key in documentTime.Keys)
        {
          if (result.ContainsKey(key))
          {
            result[key] += documentTime[key];
            count[key]++;
          }
          else
          {
            result[key] = documentTime[key];
            count[key] = 1;
          }
        }
      }

      foreach (var key in count.Keys)
      {
        result[key] /= count[key];
      }

      return result;
    }

    public IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(IQueryable<Document> documents)
    {
      var histories = documents.Select(d => d.History);
      return AverageTimeBetweenStatusChanges(histories);
    }  
  }
}
