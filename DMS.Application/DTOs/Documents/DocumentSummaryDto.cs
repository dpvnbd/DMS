using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Documents
{
  public class DocumentSummaryDto
  {
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public DocumentStatus CurrentDocumentStatus { get; set; }
  }
}
