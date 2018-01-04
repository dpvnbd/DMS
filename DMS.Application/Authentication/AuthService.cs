using DMS.Application.DTOs.Authentication;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Authentication
{
  public class AuthService : IAuthService
  {
    private SignInManager<AppIdentityUser> signInManager;
    private UserManager<AppIdentityUser> userManager;
    private IRepository<AppUser> userRepo;

    public AuthService(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager,
       IRepository<AppUser> userRepo)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.userRepo = userRepo;
    }


    public async Task<bool> Register(RegisterDto registerDTO)
    {
      var domainUser = new AppUser(registerDTO.FirstName, registerDTO.LastName, UserRole.Customer);
      await userRepo.Create(domainUser);

      var identityUser = new AppIdentityUser
      {
        UserName = registerDTO.UserName,
        Email = registerDTO.Email,
        AppUser = domainUser
      };

      var result = await userManager.CreateAsync(identityUser, registerDTO.Password);
      if (!result.Succeeded)
      {
        await userRepo.Delete(domainUser.Id);
      }

      return result.Succeeded;
    }

    public async Task<bool> Login(LoginDto loginDTO)
    {
      var result = await signInManager.PasswordSignInAsync(
          loginDTO.Username, loginDTO.Password,
          isPersistent: true, lockoutOnFailure: false);
      return result.Succeeded;
    }

    public async Task Logout()
    {
      await signInManager.SignOutAsync();
    }
  }
}
