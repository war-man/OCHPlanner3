using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OCHPlanner3.Data;
using OCHPlanner3.Data.Factory;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Mapper;
using OCHPlanner3.Helper;
using OCHPlanner3.Services;
using OCHPlanner3.Services.Email;
using OCHPlanner3.Services.Email.Entities;
using OCHPlanner3.Services.Interfaces;
using System;

namespace OCHPlanner3
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AuthConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(480);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            // Services
            services.AddTransient<IReferenceService, ReferenceService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGarageService, GarageService>();
            services.AddTransient<IOptionService, OptionService>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IVINQueryService, VINQueryService>();
            services.AddTransient<IMaintenanceTypeService, MaintenanceTypeService>();
            services.AddTransient<IVehicleService, VehicleService>();

            // Factory
            services.AddTransient<IReferenceFactory, ReferenceFactory>();
            services.AddTransient<IGarageFactory, GarageFactory>();
            services.AddTransient<IOptionFactory, OptionFactory>();
            services.AddTransient<IVehicleFactory, VehicleFactory>();
            services.AddTransient<IMaintenanceTypeFactory, MaintenanceTypeFactory>();
            services.AddTransient<IVehicleFactory, VehicleFactory>();


            // Email Sender
            services.AddSingleton<IEmailSender, EmailSender>();

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
               

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            GarageMapper.ConfigGarageMapper();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            LocalizationPipeline.ConfigureOptions(options.Value);
            app.UseRequestLocalization(options.Value);

            app.UseExceptionless("d6tk47QLBGbUHdBkD0VkhQr4J2svHRpyqYo5TLbF");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{lang:lang}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
