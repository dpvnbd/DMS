using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Identity
{
  public class AppIdentityRole : MongoIdentityRole
  {
    public AppIdentityRole() : base()
    {
    }

    public AppIdentityRole(string roleName) : base(roleName)
    {
    }
  }
}
