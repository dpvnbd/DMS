using AspNetCore.Identity.MongoDbCore.Models;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Infrastructure.Data.Identity
{
  public class AppIdentityUser : MongoIdentityUser
  {
    public int AppUserId { get; set; }

    public AppIdentityUser() : base()
    {
    }

    public AppIdentityUser(string userName, string email) : base(userName, email)
    {
    }
  }
}
