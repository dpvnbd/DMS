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

    Task<int> CreateDocument(DocumentContentsDto document, int authorId);

    Task<int> CreateDocument(DocumentContentsDto document, int authorId, int userActingOnBehalfId);

    Task<bool> EditDocument(DocumentContentsDto document, int documentId, int editorId);

    Task<DocumentWithHistoryDto> GetFullDocument(int id, int requestingUserId = -1);

    Task<DocumentContentsDto> GetDocumentContents(int id);

    Task<bool> ChangeStatus(int documentId, int editorId, DocumentStatus status, string message);

    Task<DocumentSummaryDto> GetDocumentSummary(int id);

    Task<IEnumerable<DocumentSummaryDto>> FindDocuments(int authorId = -1, DocumentStatus? status = null,
      string searchString = null, int requestingUserId = -1, bool canBeReviewed = false,
      DateTime? fromDate = null, DateTime? toDate = null);

    Task<bool> Delete(int documentId);

  }
}
