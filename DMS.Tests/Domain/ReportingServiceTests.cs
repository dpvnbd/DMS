using DMS.Domain.Abstract.Services;
using DMS.Domain.Entities;
using DMS.Domain.Entities.Reporting;
using DMS.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.Tests.Domain
{
  [TestClass]
  public class ReportingServiceTests
  {
    [TestMethod]
    public void TimeBetweenStatusChanges_three_statuses_return_two_changes()
    {
      var service = new ReportingService(null, null);
      var author = new AppUser("Test", "User", UserRole.Operator);
      var now = DateTime.Now;
      var documentHistory = new List<DocumentHistoryEntry>
      {
        new DocumentHistoryEntry(author, DocumentStatus.Created, "", now),
        new DocumentHistoryEntry(author, DocumentStatus.Registered, "", now + TimeSpan.FromMinutes(5)),
        new DocumentHistoryEntry(author, DocumentStatus.Approved, "", now + TimeSpan.FromMinutes(10)),
      };

      var createdRegistered = new StatusChange(DocumentStatus.Created, DocumentStatus.Registered);
      var registeredApproved = new StatusChange(DocumentStatus.Registered, DocumentStatus.Approved);

      var stats = service.TimeBetweenStatusChanges(documentHistory);

      Assert.AreEqual(2, stats.Count);
      Assert.IsNotNull(stats[createdRegistered]);
      Assert.IsNotNull(stats[registeredApproved]);
      Assert.AreEqual(5, stats[createdRegistered].TotalMinutes);
      Assert.AreEqual(5, stats[registeredApproved].TotalMinutes);
    }

    [TestMethod]
    public void TimeBetweenStatusChanges_editing_excluded()
    {
      var service = new ReportingService(null, null);
      var author = new AppUser("Test", "User", UserRole.Operator);
      var now = DateTime.Now;
      var documentHistory = new List<DocumentHistoryEntry>
      {
        new DocumentHistoryEntry(author, DocumentStatus.Created, "", now),
        new DocumentHistoryEntry(author, null, "Edited", now + TimeSpan.FromMinutes(5)),
        new DocumentHistoryEntry(author, DocumentStatus.Registered, "", now + TimeSpan.FromMinutes(10)),
        new DocumentHistoryEntry(author, null, "Edited again", now + TimeSpan.FromMinutes(15)),
        new DocumentHistoryEntry(author, DocumentStatus.Approved, "", now + TimeSpan.FromMinutes(20)),
      };

      var createdRegistered = new StatusChange(DocumentStatus.Created, DocumentStatus.Registered);
      var registeredApproved = new StatusChange(DocumentStatus.Registered, DocumentStatus.Approved);

      var stats = service.TimeBetweenStatusChanges(documentHistory);

      Assert.AreEqual(2, stats.Count);
      Assert.IsNotNull(stats[createdRegistered]);
      Assert.IsNotNull(stats[registeredApproved]);
      Assert.AreEqual(10, stats[createdRegistered].TotalMinutes);
      Assert.AreEqual(10, stats[registeredApproved].TotalMinutes);
    }

    [TestMethod]
    public void AverageTimeBetweenStatusChanges_multiple_documents_unique_change()
    {
      var service = new ReportingService(null, null);
      var author = new AppUser("Test", "User", UserRole.Operator);
      var now = DateTime.Now;

      var documentHistory1 = new List<DocumentHistoryEntry>
      {
        new DocumentHistoryEntry(author, DocumentStatus.Created, "", now),
        new DocumentHistoryEntry(author, DocumentStatus.Registered, "", now + TimeSpan.FromMinutes(10)),
        new DocumentHistoryEntry(author, DocumentStatus.Approved, "", now + TimeSpan.FromMinutes(20)),
        new DocumentHistoryEntry(author, DocumentStatus.Canceled, "", now + TimeSpan.FromMinutes(30)),
      };

      var documentHistory2 = new List<DocumentHistoryEntry>
      {
        new DocumentHistoryEntry(author, DocumentStatus.Created, "", now),
        new DocumentHistoryEntry(author, DocumentStatus.Registered, "", now + TimeSpan.FromMinutes(20)),
        new DocumentHistoryEntry(author, DocumentStatus.Approved, "", now + TimeSpan.FromMinutes(60)),
      };

      var createdRegistered = new StatusChange(DocumentStatus.Created, DocumentStatus.Registered);
      var registeredApproved = new StatusChange(DocumentStatus.Registered, DocumentStatus.Approved);
      var approvedCanceled = new StatusChange(DocumentStatus.Approved, DocumentStatus.Canceled);

      var documents = new List<IEnumerable<DocumentHistoryEntry>>
      {
        documentHistory1, documentHistory2
      };

      var stats = service.AverageTimeBetweenStatusChanges(documents.AsQueryable());

      Assert.AreEqual(3, stats.Count);
      Assert.IsNotNull(stats[createdRegistered]);
      Assert.IsNotNull(stats[registeredApproved]);
      Assert.IsNotNull(stats[approvedCanceled]);
      Assert.AreEqual(15, stats[createdRegistered].TotalMinutes);
      Assert.AreEqual(25, stats[registeredApproved].TotalMinutes);
      Assert.AreEqual(10, stats[approvedCanceled].TotalMinutes);
    }
  }
}
