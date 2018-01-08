﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Identity
{
  public class AppIdentityRole : IdentityRole
  {
    protected AppIdentityRole()
    {
    }

    public AppIdentityRole(string roleName) : base(roleName)
    {
    }
  }
}
