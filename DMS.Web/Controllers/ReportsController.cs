using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Reports;
using DMS.Web.Models.Reports;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  public class ReportsController : Controller
  {
    private readonly IAppReportsService reportsService;

    public ReportsController(IAppReportsService reportsService)
    {
      this.reportsService = reportsService;
    }

    public async Task<IActionResult> StatusCount(int userId = -1, DateTime? from = null, DateTime? to = null)
    {
      var model = await reportsService.GetStatusCount(userId, from, to);
      var filter = new ReportsFilterViewModel
      {
        UserId = userId,
        From = from,
        To = to
      };

      ViewData["filter"] = filter;
      return View(model);
    }

    public async Task<IActionResult> ReviewTime(int documentId = -1, DateTime? from = null, DateTime? to = null)
    {
      var model = await reportsService.GetStatusChangesTime(documentId, from, to);
      var filter = new ReportsFilterViewModel
      {
        DocumentId = documentId,
        From = from,
        To = to
      };

      ViewData["filter"] = filter;
      return View(model);
    }
  }
}