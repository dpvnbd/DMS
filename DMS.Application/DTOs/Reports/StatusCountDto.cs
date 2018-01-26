using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Reports
{
  public class StatusCountDto
  {
    public UserSummaryDto TargetUser { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public IDictionary<DocumentStatus, int> DocumentsCurrentStatusCount { get; set; }
    public int TotalDocuments { get; set; }

    public IDictionary<DocumentStatus, int> StatusChangesCount { get; set; }
    public int TotalChanges { get; set; }

  }
}
