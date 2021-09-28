using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSORequestApplication.Models;
using Microsoft.EntityFrameworkCore;
using SSORequestApplication.HelperClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using AspNetCore.Security.CAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace SSORequestApplication
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EmailHelper>();
            services.AddControllersWithViews();
            services.AddDbContext<ServicesRegistryContext>(options => options.UseSqlServer(configuration.GetConnectionString("SsoDb")));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/login");
                })
                .AddCAS(options =>
                {
                    options.CasServerUrlBase = configuration["CasBaseUrl"];   // Set in `appsettings.json` file.
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });

            var globalAuthPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(globalAuthPolicy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {// If in a development environment, use helpful exception pages that show raw error and stack info
                app.UseDeveloperExceptionPage();
            } else
            {// Otherwise, use the generic error page that production users should see.
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error/{0}"); // Status code pages will run a view like "404.cshtml"
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ssorequest}/{action=create}");
                // The view given to users that go to sso.sampleuniversity.edu/request
            });
        }
    }
}
