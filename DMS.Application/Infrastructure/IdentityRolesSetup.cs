using DMS.Application.Authentication;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.Infrastructure
{
  public static class IdentityRolesSetup
  {
    public static void CreateRoles(IServiceProvider serviceProvider)
    {
      using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {

        //initializing custom roles 
        var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
        var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
        var userRepo = scope.ServiceProvider.GetRequiredService<IRepository<AppUser>>();
        var roleNames = new List<string>();
        foreach(var role in Enum.GetValues(typeof(AppUserIdentityRoleEnum)))
        {
          roleNames.Add(role.ToString());
        }
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
          var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
          // ensure that the role does not exist
          if (!roleExist)
          {
            //create the roles and seed them to the database: 
            roleResult = RoleManager.CreateAsync(new AppIdentityRole(roleName)).Result;
          }
        }

        // find the user with the admin email 
        var _user = UserManager.FindByNameAsync("admin").Result;

        // check if the user exists
        if (_user == null)
        {
          //Here you could create the super admin who will maintain the web app

          var domainUser = new AppUser("Admin", "Admin", UserRole.Expert);

          userRepo.Create(domainUser).Wait();

          var poweruser = new AppIdentityUser
          {
            UserName = "admin",
            Email = "admin@email.com",
            AppUserId = domainUser.Id
          };
          string adminPassword = "DefaultAdminPassword";

          var createPowerUser = UserManager.CreateAsync(poweruser, adminPassword).Result;
          if (createPowerUser.Succeeded)
          {
            //here we tie the new user to the role
            var rolesResult = UserManager.AddToRoleAsync(poweruser, AppUserIdentityRoleEnum.Admin.ToString()).Result;
          }
        }
      }
    }
  }
}
