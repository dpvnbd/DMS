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
    private readonly IRepository<DocumentHistoryEntry> historyRepo;
    private readonly IRepository<AppUser> userRepo;
    private readonly IDocumentFilteringService filteringService;
    private readonly IMapper mapper;

    public AppReportsService(IReportingService reportingService, IRepository<Document> docRepo,
      IRepository<DocumentHistoryEntry> historyRepo, IRepository<AppUser> userRepo,
      IDocumentFilteringService filteringService, IMapper mapper)
    {
      this.reportingService = reportingService;
      this.docRepo = docRepo;
      this.historyRepo = historyRepo;
      this.userRepo = userRepo;
      this.filteringService = filteringService;
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
        result.StatusChangesTime = reportingService.TimeBetweenStatusChanges(document.History);
      }
      else
      {
        var documents = filteringService.Filter(docRepo.GetAll(), fromDate: fromDate, toDate: toDate);
        result.StatusChangesTime = reportingService.AverageTimeBetweenStatusChanges(documents);
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

      var documents = filteringService.Filter(docRepo.GetAll(), user, fromDate: fromDate, toDate: toDate);

      result.DocumentsCurrentStatusCount = reportingService.CountDocumentsByStatus(documents);
      result.TotalDocuments = result.DocumentsCurrentStatusCount.Values.Sum();

      // TODO - move status filtering logic to another place
      bool historyPredicate(DocumentHistoryEntry e)
      {
        if (user != null && e.User.Id != user.Id)
        {
          return false;
        }

        if (fromDate.HasValue && e.Created < fromDate.Value)
        {
          return false;
        }

        if (toDate.HasValue && e.Created > toDate.Value)
        {
          return false;
        }

        return true;
      }

      var statusChanges = historyRepo.GetAll().Where(historyPredicate);
      result.StatusChangesCount = reportingService.CountStatusChanges(statusChanges.AsQueryable());
      result.TotalChanges = result.StatusChangesCount.Values.Sum();


      return result;
    }
  }
}
