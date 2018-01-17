using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Authentication;
using DMS.Application.Users;
using DMS.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  [Authorize]
  public class UserController : Controller
  {
    private readonly IAppUserService userService;
    private readonly IAuthService authService;

    public UserController(IAppUserService userService, IAuthService authService)
    {
      this.userService = userService;
      this.authService = authService;
    }

    public async Task<IActionResult> Index()
    {
      var userId = await authService.GetUserIdByClaims(User);
      var users = await userService.FindUsers(u => true, userId);
      return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
      var currentUserId = await authService.GetUserIdByClaims(User);

      var user = await userService.GetUser(id, currentUserId);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Identity(int id)
    {
      var user = await authService.GetUserIdentity(id);
      if (user == null)
      {
        return NotFound();
      }

      ViewData["User"] = user;

      var model = new UserRolesViewModel
      {
        Id = user.AppUser.Id,
        IdentityId = user.Id
      };
      return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> SetRoles(UserRolesViewModel model)
    {
      var user = await authService.GetUserIdentity(model.Id);
      if (user == null)
      {
        return NotFound();
      }

      ViewData["User"] = user;

      if (ModelState.IsValid)
      {
        var identityResult = await authService.SetUserIdentityRole(model.IdentityId, model.IdentityRole);
        var userResult = await userService.SetRole(model.Id, model.UserRole);

        if (!identityResult || !userResult)
        {
          ModelState.AddModelError(string.Empty, "Unable to change some of the roles");
        }
      }


      return RedirectToAction("Identity", new { id = model.Id });

    }
  }
}