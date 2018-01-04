using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.Authentication;
using DMS.Application.DTOs.Authentication;
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
    private readonly IAuthService authService;
    private readonly IMapper mapper;

    public AccountController(IAuthService authService, IMapper mapper)
    {
      this.authService = authService;
      this.mapper = mapper;
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
      {
        return View(model);
      }

      var registerDto = mapper.Map<RegisterDto>(model);

      var success = await authService.Register(registerDto);
      if (success)
      {
        return RedirectToAction("Login", "Account");
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
      {
        return View(model);
      }

      var loginDto = mapper.Map<LoginDto>(model);

      var success = await authService.Login(loginDto);

      if (success)
      {
        return RedirectToAction("Index", "Home");
      }

      ModelState.AddModelError(string.Empty, "Login Failed");
      return View(model);
    }

    public async Task<IActionResult> Logout()
    {
      await authService.Logout();
      return RedirectToAction("Index", "Home");
    }
  }
}