using AutoMapper;
using DMS.Application.DTOs.Authentication;
using DMS.Application.DTOs.Users;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Authentication
{
  public class AuthService : IAuthService
  {
    private SignInManager<AppIdentityUser> signInManager;
    private UserManager<AppIdentityUser> userManager;
    private IRepository<AppUser> userRepo;
    private readonly IRepository<AppIdentityUser> identityRepo;
    private readonly IMapper mapper;

    public AuthService(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager,
       IRepository<AppUser> userRepo, IMapper mapper)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.userRepo = userRepo;
      this.mapper = mapper;
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

    public async Task<bool> ChangePassword(ClaimsPrincipal user, string oldPassword, string newPassword)
    {
      var identityUser = await userManager.GetUserAsync(user);
      if (identityUser == null)
      {
        return false;
      }

      var result = await userManager.ChangePasswordAsync(identityUser, oldPassword, newPassword);
      return result.Succeeded;
    }

    public async Task<UserSummaryDto> GetUserByClaims(ClaimsPrincipal userClaim)
    {
      var userId = await GetUserIdByClaims(userClaim);
      var user = await userRepo.GetById(userId);
      var dto = mapper.Map<UserSummaryDto>(user);
      return dto;
    }

    public async Task<int> GetUserIdByClaims(ClaimsPrincipal userClaim)
    {
      if (userClaim == null)
      {
        return -1;
      }

      var identityUser = await userManager.GetUserAsync(userClaim);

      return identityUser?.AppUserId ?? -1;
    }

    public async Task<bool> SetUserIdentityRole(string userId, AppUserIdentityRoleEnum role)
    {
      var user = await userManager.FindByIdAsync(userId);
      if(user == null)
      {
        return false;
      }

      if(user.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
      {
        return false; // keep at least one admin
      }

      var roles = await userManager.GetRolesAsync(user);
      await userManager.RemoveFromRolesAsync(user, roles);

      var result = await userManager.AddToRoleAsync(user, role.ToString());

      return result.Succeeded;
    }

    public async Task<UserIdentityDto> GetUserIdentity(int id)
    {
      var user = userManager.Users.FirstOrDefault(i => i.AppUserId == id);
      if (user == null)
      {
        return null;
      }
      user.AppUser = await userRepo.GetById(user.AppUserId);
      
      var dto = mapper.Map<UserIdentityDto>(user);

      dto.IdentityRoles = await userManager.GetRolesAsync(user);
      return dto;
    }
  }  
}
