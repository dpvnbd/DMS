using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Reports;
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
      return View(model);
    }

    public async Task<IActionResult> ReviewTime(int documentId = -1, DateTime? from = null, DateTime? to = null)
    {
      var model = await reportsService.GetStatusChangesTime(documentId, from, to);
      return View(model);
    }
  }
}