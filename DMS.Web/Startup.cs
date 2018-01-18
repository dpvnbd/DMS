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
using DMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
      services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DMS")));

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
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
      services.AddScoped<IRepository<Document>, DocumentRepository>();

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
