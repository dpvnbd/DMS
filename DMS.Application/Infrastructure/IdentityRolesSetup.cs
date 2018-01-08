using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Infrastructure
{
  public static class IdentityRolesSetup
  {
    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
      //initializing custom roles 
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
      string[] roleNames = { "Admin", "Moderator", "Default" };
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        // ensure that the role does not exist
        if (!roleExist)
        {
          //create the roles and seed them to the database: 
          roleResult = await RoleManager.CreateAsync(new AppIdentityRole(roleName));
        }
      }

      // find the user with the admin email 
      var _user = await UserManager.FindByNameAsync("admin");

      // check if the user exists
      if (_user == null)
      {

        //Here you could create the super admin who will maintain the web app

        var domainUser = new AppUser("Admin", "Admin", UserRole.Expert);

        var poweruser = new AppIdentityUser
        {
          UserName = "admin",
          Email = "admin@email.com",
          AppUser = domainUser
        };
        string adminPassword = "DefaultAdminPassword";

        var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
        if (createPowerUser.Succeeded)
        {
          //here we tie the new user to the role
          await UserManager.AddToRoleAsync(poweruser, "Admin");
        }
      }
    }
  }
  }
}
