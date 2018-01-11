using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Documents
{
  public class DocumentSummaryDto
  {
    public int Id { get; set; }
    public UserSummaryDto Author { get; set; }
    public string Title { get; set; }
    public DocumentStatus CurrentDocumentStatus { get; set; }
    public bool CanEdit { get; set; }
  }
}
