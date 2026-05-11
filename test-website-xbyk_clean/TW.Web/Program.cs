using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// Enable desired Kentico Xperience features
builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        // Add your page-builder-enabled content type names here, e.g.
        // ContentTypeNames = new[] { HomePage.CONTENT_TYPE_NAME, ContentPage.CONTENT_TYPE_NAME },
        // Set DefaultSectionIdentifier once you register a section, e.g.
        // DefaultSectionIdentifier = DefaultSection.Identifier,
    });
    features.UseWebPageRouting();
    // features.UseActivityTracking();
    // features.UseEmailStatisticsLogging();
    // features.UseEmailMarketing();
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

// Register custom application services here, e.g.
// builder.Services.AddScoped<ISiteConfigurationRepository, SiteConfigurationRepository>();

var app = builder.Build();
app.InitKentico();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseAuthentication();


app.UseKentico();

app.UseAuthorization();

app.Kentico().MapRoutes();

app.Run();
