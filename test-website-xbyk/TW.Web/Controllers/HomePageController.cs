using System.Threading.Tasks;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Controllers;
using TW.Web.Generated;
using TW.Web.Models;
using TW.Web.Services;

[assembly: RegisterWebPageRoute(
    contentTypeName: HomePage.CONTENT_TYPE_NAME,
    controllerType: typeof(HomePageController))]

namespace TW.Web.Controllers;

public class HomePageController : Controller
{
    private readonly IContentRetriever contentRetriever;

    public HomePageController(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IActionResult> Index()
    {
        var page = await contentRetriever.RetrieveCurrentPage<HomePage>(
            new RetrieveCurrentPageParameters { LinkedItemsMaxLevel = 1 });

        var model = new HomePageViewModel
        {
            Name = page.HomePageName,
            Subheading = page.HomePageSubheading,
            HeroImageUrl = MediaHelper.FirstUrl(page.HomePageHeroImage),
            MetaTitle = page.PageMetaTitle,
            MetaDescription = page.PageMetaDescription,
        };

        return View(model);
    }
}
