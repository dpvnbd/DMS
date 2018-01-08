using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  [Authorize]
  public class UserController : Controller
  {
    private readonly IAppUserService userService;

    public UserController(IAppUserService userService)
    {
      this.userService = userService;
    }

    public IActionResult Index()
    {
      var users = userService.FindUsers(u => true);
      return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
      var user = await userService.GetUser(id);
      if (userService == null)
      {
        return NotFound();
      }

      return View(user);
    }
  }
}