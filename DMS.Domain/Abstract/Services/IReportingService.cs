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
    IDictionary<DocumentStatus, int> CountDocumentsByStatus(AppUser user = null,
      DateTime? fromDate = null, DateTime? toDate = null);

    IDictionary<DocumentStatus, int> CountStatusChanges(IQueryable<DocumentHistoryEntry> historyEntries);
    IDictionary<DocumentStatus, int> CountStatusChanges(AppUser user = null,
      DateTime? fromDate = null, DateTime? toDate = null);

    IDictionary<StatusChange, TimeSpan> TimeBetweenStatusChanges(IEnumerable<DocumentHistoryEntry> history);
    IDictionary<StatusChange, TimeSpan> TimeBetweenStatusChanges(Document document);

    IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(
      IQueryable<IEnumerable<DocumentHistoryEntry>> documentHistories);
    IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(IQueryable<Document> documents);
    IDictionary<StatusChange, TimeSpan> AverageTimeBetweenStatusChanges(DateTime? fromDate = null, DateTime? toDate = null);
  }
}
