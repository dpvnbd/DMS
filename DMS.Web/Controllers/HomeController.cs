using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  public class HomeController : Controller
  {
    public HomeController()
    {
    }
    public IActionResult Index()
    {
      return RedirectToAction("Index", "Document");
    }
  }
}