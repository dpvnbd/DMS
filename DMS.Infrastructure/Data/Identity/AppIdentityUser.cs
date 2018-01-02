using DMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Identity
{
  public class AppIdentityUser : IdentityUser
  {
    public AppUser AppUser { get; set; }
  }
}
