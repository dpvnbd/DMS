using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Documents
{
  public class HistoryEntryDto
  {
    public UserSummaryDto User { get; set; }
    public UserSummaryDto UserActingOnBehalf { get; set; }
    public DocumentStatus? Status { get; set; }
    public string Message { get; set; }
    public DateTime Created { get; protected set; }
  }
}
