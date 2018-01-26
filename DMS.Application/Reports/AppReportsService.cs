using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.DTOs.Documents;
using DMS.Application.DTOs.Reports;
using DMS.Application.DTOs.Users;
using DMS.Domain.Abstract;
using DMS.Domain.Abstract.Services;
using DMS.Domain.Entities;

namespace DMS.Application.Reports
{
  public class AppReportsService : IAppReportsService
  {
    private readonly IReportingService reportingService;
    private readonly IRepository<Document> docRepo;
    private readonly IRepository<AppUser> userRepo;
    private readonly IMapper mapper;

    public AppReportsService(IReportingService reportingService, IRepository<Document> docRepo,
      IRepository<AppUser> userRepo, IMapper mapper)
    {
      this.reportingService = reportingService;
      this.docRepo = docRepo;
      this.userRepo = userRepo;
      this.mapper = mapper;
    }

    public async Task<StatusChangesTimeDto> GetStatusChangesTime(int forDocumentId = -1, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var result = new StatusChangesTimeDto
      {
        FromDate = fromDate,
        ToDate = toDate
      };

      Document document = null;

      if(forDocumentId != -1)
      {
        document = await docRepo.GetById(forDocumentId);
        if(document == null)
        {
          return result;
        }
        result.TargetDocument = mapper.Map<DocumentSummaryDto>(document);
      }

      if(document != null)
      {
        result.StatusChangesTime = reportingService.TimeBetweenStatusChanges(document);
      }
      else
      {
        result.StatusChangesTime = reportingService.AverageTimeBetweenStatusChanges(fromDate, toDate);
      }

      result.TotalTime = new TimeSpan(result.StatusChangesTime.Values.Sum(t => t.Ticks));
      return result;
    }

    public async Task<StatusCountDto> GetStatusCount(int forUserId = -1, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var result = new StatusCountDto
      {
        FromDate = fromDate,
        ToDate = toDate
      };

      AppUser user = null;
      if(forUserId != -1)
      {
        user = await userRepo.GetById(forUserId);
        if(user == null)
        {
          return result;
        }
        result.TargetUser = mapper.Map<UserSummaryDto>(user);
      }

      result.DocumentsCurrentStatusCount = reportingService.CountDocumentsByStatus(user, fromDate, toDate);
      result.TotalDocuments = result.DocumentsCurrentStatusCount.Values.Sum();

      result.StatusChangesCount = reportingService.CountStatusChanges(user, fromDate, toDate);
      result.TotalChanges = result.StatusChangesCount.Values.Sum();


      return result;
    }
  }
}
