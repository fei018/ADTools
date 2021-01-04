using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCenter;
using System.Security.Claims;
using System.Text.Unicode;

namespace ActiveDirectoryTools
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
            services.AddControllersWithViews();

            AddOtherServices(ref services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=admanage}/{action=Index}/{id?}");
            });
        }



        ////////////////

        private void AddOtherServices(ref IServiceCollection services)
        {
            // add Cookie scheme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.LoginPath = new PathString("/Login/Index");
                        option.LogoutPath = new PathString("/Login/Index");
                        option.AccessDeniedPath = new PathString("/Home/AccessDenied");
                    });

            // add policy authorize
            services.AddAuthorization(option =>
            {
                //option.AddPolicy(ServicePolicy.UserRole, policy => policy.RequireClaim(ClaimTypes.Role, LoginUserRole.User, LoginUserRole.Admin));
                
            });


            // http response html 拉丁中文不编码
            services.AddSingleton(System.Text.Encodings.Web.HtmlEncoder.Create(new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            // HttpContext 
            services.AddHttpContextAccessor();

            // ServiceCenter
            services.AddServiceCenter();

        }

    }
}
