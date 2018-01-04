using DMS.Application.DTOs.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Documents
{
  public interface IDocumentService
  {
    /// <summary>
    /// Creates a new document
    /// </summary>
    /// <param name="document">Contents of the new document</param>
    /// <returns>Id of the new document</returns>
    Task<int> CreateDocument(DocumentContentsDto document);
  }
}
