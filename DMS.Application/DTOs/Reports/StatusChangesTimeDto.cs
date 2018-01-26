using DMS.Application.DTOs.Documents;
using DMS.Application.DTOs.Users;
using DMS.Domain.Entities.Reporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Reports
{
  public class StatusChangesTimeDto
  {    
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DocumentSummaryDto TargetDocument { get; set; }
    public IDictionary<StatusChange, TimeSpan> StatusChangesTime{ get; set; }
    public TimeSpan TotalTime { get; set; }
  }
}
