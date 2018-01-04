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
    private readonly IRepository<Document> documentRepo;

    public HomeController(IRepository<Document> documentRepo)
    {
      this.documentRepo = documentRepo;
    }
    public IActionResult Index()
    {
      var documents = documentRepo.GetAll();
      return View(documents);
    }
  }
}