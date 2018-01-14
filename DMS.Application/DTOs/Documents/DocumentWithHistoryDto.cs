using DMS.Application.DTOs.Users;
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
    public UserSummaryDto Author { get; set; }
    public UserSummaryDto OnBehalfOfUser { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public IEnumerable<HistoryEntryDto> History { get; set; }
    public DocumentStatus CurrentStatus { get; set; }
    public IEnumerable<DocumentStatus> AvailableStatusChanges { get; set; }
    public bool CanEdit { get; set; }
  }
}
