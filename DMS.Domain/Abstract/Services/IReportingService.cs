using DMS.Domain.Entities;
using DMS.Domain.Entities.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Abstract.Services
{
  public interface IReportingService
  {
    IDictionary<DocumentStatus, int> CountDocumentsByStatus(IQueryable<Document> documents);
    
    IDictionary<DocumentStatus, int> CountStatusChanges(IQueryable<DocumentHistoryEntry> historyEntries);

    IDictionary<StatusChange, TimeSpan> TimeBetweenStatusChanges(IEnumerable<DocumentHistoryEntry> history);

    IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(
      IQueryable<IEnumerable<DocumentHistoryEntry>> documentHistories);
    IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(IQueryable<Document> documents);
  }
}
