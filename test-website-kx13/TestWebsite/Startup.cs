using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TestWebsite.Sections.DefaultSection;
using TestWebsite.Services;

namespace TestWebsite
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var kenticoServiceCollection = services.AddKentico(features =>
            {
                features.UsePageBuilder(new PageBuilderOptions
                {
                    DefaultSectionIdentifier = DefaultSection.Identifier,
                });
                features.UsePageRouting();
            });

            if (Environment.IsDevelopment())
            {
                kenticoServiceCollection.SetAdminCookiesSameSiteNone();
                kenticoServiceCollection.DisableVirtualContextSecurityForLocalhost();
            }

            services.AddScoped<ISiteConfigurationRepository, SiteConfigurationRepository>();

            services.AddAuthentication();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseKentico();

            app.UseCookiePolicy();
            app.UseCors();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Kentico().MapRoutes();
            });
        }
    }
}
