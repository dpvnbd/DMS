using DMS.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Reports
{
  public interface IAppReportsService
  {
    Task<StatusCountDto> GetStatusCount(int forUserId = -1, DateTime? fromDate = null, DateTime? toDate = null);

    Task<StatusChangesTimeDto> GetStatusChangesTime(int forDocumentId = -1, DateTime? fromDate = null, DateTime? toDate = null);
  }
}
