using DMS.Application.DTOs.Documents;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Documents
{
  public interface IAppDocumentService
  {
    /// <summary>
    /// Creates a new document
    /// </summary>
    /// <param name="document">Contents of the new document</param>
    /// <returns>Id of the new document</returns>
    Task<int> CreateDocument(DocumentContentsDto document);

    Task<DocumentWithHistoryDto> GetDocument(int id, int userId);

    Task<bool> ChangeStatus(int documentId, int appUserId, DocumentStatus status, string message);

    IEnumerable<DocumentSummaryDto> FindDocuments(Func<Document, bool> predicate);
  }
}
