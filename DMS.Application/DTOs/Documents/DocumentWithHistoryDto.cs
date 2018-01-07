using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Documents
{
  public class DocumentWithHistoryDto
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public AppUserDto Author { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public IEnumerable<StatusChangeDto> History { get; set; }
    public DocumentStatus CurrentDocumentStatus { get; set; }
    public IEnumerable<DocumentStatus> AvailableStatusChanges { get; set; }
  }
}
