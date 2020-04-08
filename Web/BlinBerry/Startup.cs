using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using BlinBerry.Data;
using BlinBerry.Data.Models.IdentityModels;
using BlinBerry.Data.Repositories;
using BlinBerry.Data.Seedeng;
using BlinBerry.Service.AccountServise;
using BlinBerry.Service.CashAndProducts;
using BlinBerry.Service.MapperProfile;
using BlinBerry.Service.ProcurementService;
using BlinBerry.Service.RecipeServise;
using BlinBerry.Service.SelesReportService;
using BlinBerry.Service.SpendingService;
using BlinBerry.Services.Common.AccountService;
using BlinBerry.Services.Common.CommonInfoService;
using BlinBerry.Services.Common.CommonInfoService.Models;
using BlinBerry.Services.Common.ProcurementService;
using BlinBerry.Services.Common.RecipeService;
using BlinBerry.Services.Common.SelesReport;
using BlinBerry.Services.Common.SpendingService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlinBerry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<RequestLocalizationOptions>(
            //    opts =>
            //    {
            //        var supportedCultures = new List<CultureInfo>
            //        {
            //            new CultureInfo("ky"),
            //            new CultureInfo("ru"),
            //        };

            //        opts.DefaultRequestCulture = new RequestCulture("ru-RU");
            //        opts.SupportedCultures = supportedCultures;
            //        opts.SupportedUICultures = supportedCultures;
            //    });


            services.AddDbContext<ApplicationDbContext>(
               options =>
               {
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    //options.UseLazyLoadingProxies();
                });

            services
             .AddIdentity<ApplicationUser, Role>(options =>
             {
                 options.Password.RequireDigit = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 3;
             })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(MapperProfile));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<IAccountService, AccountService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<ISelesReportService, SelesReportService>();
            services.AddTransient<ICommonInfoAboutAccountService, CommonAccountService>();
            services.AddTransient<IProcurementService, ProcurementService>();
            services.AddTransient<ISpendingService, SpendingService>();
            services.AddTransient<IRecipeService, RecipeService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    if (env.IsDevelopment())
                    {
                        dbContext.Database.Migrate();
                    }

                    new UserSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
