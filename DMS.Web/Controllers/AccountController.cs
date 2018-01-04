using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using DMS.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  public class AccountController : Controller
  {
    private readonly SignInManager<AppIdentityUser> signInManager;
    private readonly UserManager<AppIdentityUser> userManager;
    private readonly IRepository<AppUser> userRepo;

    public AccountController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager,
      IRepository<AppUser> userRepo)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.userRepo = userRepo;
    }

    [HttpGet]
    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      var identityUser = new AppIdentityUser
      {
        UserName = model.UserName,
        Email = model.Email,
      };

      var domainUser = new AppUser(model.FirstName, model.LastName, UserRole.Customer);


      var result = await this.userManager.CreateAsync(identityUser, model.Password);
      if (result.Succeeded)
      {
        await userRepo.Create(domainUser);

        identityUser.AppUser = domainUser;
        var linkDomainUserResult = await userManager.UpdateAsync(identityUser);

        if (linkDomainUserResult.Succeeded)
        {
          return RedirectToAction("Index", "Home");
        }
        else
        {
          await userManager.DeleteAsync(identityUser);
        }
      }

      return View(model);
    }

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      var result = await signInManager.PasswordSignInAsync(
          model.Username, model.Password,
          isPersistent: true, lockoutOnFailure: false);

      if (result.Succeeded)
        return RedirectToAction("Index", "Home");

      ModelState.AddModelError(string.Empty, "Login Failed");
      return View(model);
    }

    public async Task<IActionResult> Logout()
    {
      await signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}