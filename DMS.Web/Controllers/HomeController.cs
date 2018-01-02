using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      var documents = new List<Document>
      {
        new Document("Title", "Body", new AppUser("Test", "User", UserRole.Customer)),
        new Document("Title2", "Body2", new AppUser("Test", "User", UserRole.Customer)),        
      };
      return View(documents);
    }
  }
}