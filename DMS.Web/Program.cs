using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DMS.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = BuildWebHost(args);
      using (var scope = host.Services.CreateScope())
      {
        // place your DB seeding code here
        IdentityRolesSetup.CreateRoles(scope.ServiceProvider);
      }
      host.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
  }
}
