﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdminSystem.Common;
using AdminSystem.IRepositories.Admin;
using AdminSystem.Models.Admin.Infrastructure;
using AdminSystem.Models.Admin.MyDbContext;
using AdminSystem.Repositories.Admin;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using Pivotal.Discovery.Client;

namespace AdminSystem.Api
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public static IContainer AutofacContainer { get; set; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);
            services.AddDbContext<IUnitOfWork,AdminDbContext>(o => o.UseMySql(Configuration.GetConnectionString("mysqlconnectionString"), b => b.MigrationsAssembly("AdminSystem.Api")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IUnitOfWork unitOfWork)
        {
            
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePages();


            app.UseAuthentication();
            app.UseMvc();
            app.UseDiscoveryClient();
            
        }
    }
}
