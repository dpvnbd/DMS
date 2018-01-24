using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.Authentication;
using DMS.Application.Documents;
using DMS.Application.Infrastructure;
using DMS.Application.Users;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure;
using DMS.Infrastructure.Data;
using DMS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCore.Identity.MongoDbCore;
using DMS.Infrastructure.Repositories;
using MongoDB.Driver;
using DMS.Infrastructure.Data.MongoDb;

namespace DMS.Web
{
  public class Startup
  {
    private IConfiguration Configuration;
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      MongoMappings.SetupMappings();

      var connectionString = Configuration.GetConnectionString("DMS");
      var dbName = Configuration["MongoDbName"];

      services.AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
      {
        // Password settings
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 1;
      })
      .AddMongoDbStores<AppIdentityUser, AppIdentityRole, Guid>(
       connectionString,
       dbName
      ).AddDefaultTokenProviders();


      services.AddSingleton<IMongoClient, MongoClient>(p => new MongoClient(connectionString));
      services.AddScoped(p =>
      {
        var client = p.GetRequiredService<IMongoClient>();
        return client.GetDatabase(dbName);
      });


      services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IAppDocumentService, AppDocumentService>();
      services.AddScoped<IAppUserService, AppUserService>();

      services.AddAutoMapper();

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      app.UseStaticFiles();
      app.UseMvcWithDefaultRoute();
    }
  }
}
