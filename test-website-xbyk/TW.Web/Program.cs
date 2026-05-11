using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TW.Web.Generated;
using TW.Web.Sections.DefaultSection;
using TW.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        DefaultSectionIdentifier = DefaultSection.Identifier,
        ContentTypeNames = new[]
        {
            HomePage.CONTENT_TYPE_NAME,
            ContentPage.CONTENT_TYPE_NAME,
        },
    });
    features.UseWebPageRouting();
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISiteConfigurationRepository, SiteConfigurationRepository>();

var app = builder.Build();
app.InitKentico();

app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseKentico();
app.UseAuthorization();

app.Kentico().MapRoutes();

app.Run();
